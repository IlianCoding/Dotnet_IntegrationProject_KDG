using System.ComponentModel.DataAnnotations;
using NT.BL.Domain.platformpck;
using NT.BL.Domain.projectpck;
using NT.BL.Domain.users;
using NT.BL.Domain.Util;
using NT.BL.services;
using NT.BL.UserMngr;
using NT.DAL.PlatformRep.HeadplatformPck;
using NT.DAL.PlatformRep.SharingplatformPck;

namespace NT.BL.PlatformMngr;

public class PlatformManager : IPlatformManager
{
    private readonly IHeadplatformRepository _headplatformRepository;
    private readonly ISharingplatformRepository _sharingplatformRepository;
    private readonly IGeneralUserManager _generalUserManager;
    private readonly CloudStorageService _storageService;

    public PlatformManager(IHeadplatformRepository headplatformRepository,
        ISharingplatformRepository sharingplatformRepository,
        IGeneralUserManager generalUserManager, CloudStorageService storageService)
    {
        _headplatformRepository = headplatformRepository;
        _sharingplatformRepository = sharingplatformRepository;
        _generalUserManager = generalUserManager;
        _storageService = storageService;
    }

    #region HeadPlatform

    public Platform GetPlatformWithAllSharingPlatforms(bool isHead)
    {
        return _headplatformRepository.ReadPlatformWithAllSharingPlatforms(isHead);
    }

    #endregion

    #region SharingPlatform

    public Platform GetSharingPlatform(long id)
    {
        return _sharingplatformRepository.ReadSharingPlatform(id);
    }

    public Platform GetSharingPlatformOfOrganizationWithAllProjects(string organizationName)
    {
        return _sharingplatformRepository.ReadSharingPlatformOfOrganizationWithAllProjects(organizationName);
    }

    public Platform GetSharingPlatformOfOrganizationUserWithAllProjects(OrganizationUser organizationUser)
    {
        return _sharingplatformRepository.ReadSharingPlatformOfOrganizationUserWithAllProjects(organizationUser);
    }

    public void AddSharingPlatform(string platformName, OrganizationUser organizationMaintainer)
    {
        Platform platform = new Platform()
        {
            IsHead = false,
            PlatformName = platformName,
            CreationDate = DateOnly.FromDateTime(DateTime.Today),
            OrganizationMaintainer = new List<OrganizationUser>()
            {
                organizationMaintainer
            },
            ProjectsAssigned = new List<Project>()
        };

        ValidationContext vc = new ValidationContext(platform);
        List<ValidationResult> results = new List<ValidationResult>();
        bool isOk = Validator.TryValidateObject(platform, vc, results, validateAllProperties: true);
        if (!isOk)
        {
            string errorMessage = "";
            foreach (ValidationResult vr in results)
            {
                errorMessage += vr.ErrorMessage + "\n";
            }

            throw new ValidationException(errorMessage);
        }

        _sharingplatformRepository.CreateSharingPlatform(platform);

        _headplatformRepository.UpdateHeadPlatform(platform);
    }

    public void ChangeSharingPlatformMaintainers(OrganizationUser organizationUser, string organization)
    {
        _sharingplatformRepository.UpdateSharingPlatformMaintainers(organizationUser, organization);
    }

    #endregion

    public void ChangeLogo(string objectName, Platform platformToBeChanged)
    {
        _storageService.RemoveMedia(platformToBeChanged.LogoObjectName);
        platformToBeChanged.LogoObjectName = objectName;
        _headplatformRepository.UpdatePlatform(platformToBeChanged);
    }

    public string GetLogoByProjectId(long projectId)
    {
      return  _sharingplatformRepository.ReadLogoByProjectId(projectId);
    }

    public string GetLogoByStepId(long stepId)
    {
        return  _sharingplatformRepository.ReadLogoByStepId(stepId);
    }


    public Platform GetSharingPlatformOnly(long platformId)
    {
        return _sharingplatformRepository.ReadSharingPlatformOnly(platformId);
    }
}