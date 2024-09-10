using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.platformpck;
using NT.BL.Domain.projectpck;
using NT.BL.Domain.questionpck.AnswerDomPck;
using NT.BL.Domain.questionpck.QuestionDomPck;
using NT.BL.Domain.sessionpck;
using NT.BL.Domain.users;
using NT.BL.Domain.webplatformpck;

namespace NT.DAL.EF;

public class DataSeeder
{
    public static void Seed(PhygitalDbContext phygitalDbContext, UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        var headOfPlatform = new IdentityRole(CustomIdentityConstants.HeadOfPlatform);
        roleManager.CreateAsync(headOfPlatform).Wait();
        var organization = new IdentityRole(CustomIdentityConstants.Organization);
        roleManager.CreateAsync(organization).Wait();
        var attendent = new IdentityRole(CustomIdentityConstants.Attendent);
        roleManager.CreateAsync(attendent).Wait();
        var application = new IdentityRole(CustomIdentityConstants.Application);
        roleManager.CreateAsync(application).Wait();

        //Primary Case: lokale verkiezingen

        #region Themes
        
        Theme headTheme = new Theme()
        {
            ThemeName = "Verkiezingen",
            ShortInformation = "Projecten gerelateerd aan nationale, regionale of lokale verkiezingen",
            IsHeadTheme = true,

            Subthemes = new List<Theme>()
            {
                new Theme
                {
                    ThemeName = "Vrije tijd",
                    ShortInformation = "Projecten met betrekking tot vrijetijdsbesteding en recreatie",
                    Subthemes = null
                },
                new Theme
                {
                    ThemeName = "Sport",
                    ShortInformation = "Projecten met betrekking tot sportbeleid en -infrastructuur",
                    Subthemes = null
                },
                new Theme
                {
                    ThemeName = "Mobiliteit",
                    ShortInformation =
                        "Projecten met betrekking tot openbaar vervoer, fietsbeleid en verkeersveiligheid",
                    Subthemes = null
                },
                new Theme
                {
                    ThemeName = "Energie",
                    ShortInformation = "Projecten met betrekking tot energietransitie, duurzaamheid en klimaat",
                    Subthemes = null
                },
                new Theme
                {
                    ThemeName = "Huisvesting",
                    ShortInformation =
                        "Projecten met betrekking tot sociale woningbouw, woonbeleid en ruimtelijke ordening",
                    Subthemes = null
                },
                new Theme
                {
                    ThemeName = "Onderwijs",
                    ShortInformation =
                        "Projecten met betrekking tot onderwijsbeleid, kwaliteit van het onderwijs en onderwijsinnovatie",
                    Subthemes = null
                },
                new Theme
                {
                    ThemeName = "Volksgezondheid",
                    ShortInformation = "Projecten met betrekking tot gezondheidszorg, preventie en welzijn",
                    Subthemes = null
                },
                new Theme
                {
                    ThemeName = "Ouderenzorg",
                    ShortInformation = "Projecten met betrekking tot zorg voor ouderen en welzijn van senioren",
                    Subthemes = null
                },
                new Theme
                {
                    ThemeName = "Kiesintenties",
                    ShortInformation = "Projecten met betrekking tot onderzoek naar stemgedrag en kiezers",
                    Subthemes = null
                },
                new Theme
                {
                    ThemeName = "Stem redenen",
                    ShortInformation = "Projecten met betrekking tot onderzoek naar motivaties om te stemmen",
                    Subthemes = null
                },
                new Theme
                {
                    ThemeName = "Lokale betrokkenheid",
                    ShortInformation = "Projecten met betrekking tot burgerparticipatie en lokale democratie",
                    Subthemes = null
                },
                new Theme
                {
                    ThemeName = "Beleidsparticipatie",
                    ShortInformation = "Projecten met betrekking tot betrokkenheid van burgers bij beleidsvorming",
                    Subthemes = null
                }
            }
        };
        
        Theme headTheme2 = new Theme
        {
            ThemeName = "De Nationale Banken",
            ShortInformation = "Projecten gerelateerd aan centrale banken en het monetaire beleid",
            IsHeadTheme = true,
            Subthemes = new List<Theme>()
            {
                new Theme
                {
                    ThemeName = "Centrale banktaken", 
                    Subthemes = null,
                    ShortInformation = "Taken en verantwoordelijkheden van centrale banken"
                },
                new Theme
                {
                    ShortInformation = "Monetlair beleid", 
                    Subthemes = null,
                    ThemeName = "Beleid om de geldhoeveelheid en rentestanden te beïnvloeden"
                },
                new Theme
                {
                    ThemeName = "Financiële stabiliteit", 
                    Subthemes = null,
                    ShortInformation = "Handhaving van een stabiel financieel systeem"
                },
                new Theme
                {
                    ThemeName = "Banktoezicht", 
                    Subthemes = null,
                    ShortInformation = "Toezicht op banken en andere financiële instellingen"
                },
                new Theme
                {
                    ThemeName = "Betalingssystemen", 
                    Subthemes = null,
                    ShortInformation = "Systemen voor efficiënt betalingsverkeer"
                },
                new Theme
                {
                    ThemeName = "Contant geld", 
                    Subthemes = null,
                    ShortInformation = "Uitgifte, circulatie en beveiliging van bankbiljetten en munten"
                },
                new Theme
                {
                    ThemeName = "Financiële markten", 
                    Subthemes = null,
                    ShortInformation = "Regulering en toezicht op financiële markten"
                },
                new Theme
                {
                    ThemeName = "Financiële innovatie", 
                    Subthemes = null,
                    ShortInformation = "Stimulering van innovatie in de financiële sector"
                },
                new Theme
                {
                    ThemeName = "Financiële criminaliteit", 
                    Subthemes = null,
                    ShortInformation = "Bestrijding van witwassen en financiële criminaliteit"
                },
                new Theme
                {
                    ThemeName = "Consumentenbescherming", 
                    Subthemes = null,
                    ShortInformation = "Bescherming van consumenten op de financiële markt"
                },
                new Theme
                {
                    ThemeName = "Internationale monetaire stelsel", 
                    Subthemes = null,
                    ShortInformation = "Samenwerking om een stabiel internationaal financieel systeem te waarborgen"
                },
                new Theme
                {
                    ThemeName = "Economisch onderzoek", 
                    Subthemes = null,
                    ShortInformation = "Onderzoek naar economische ontwikkelingen en financiële stabiliteit"
                },
            }
        };

        Theme headTheme3 = new Theme()
        {
            ShortInformation = "Projects related to sports activities in Antwerp",
            ThemeName = "Sport in Antwerpen",
            IsHeadTheme = true,
            Subthemes = new List<Theme>()
            {
                new Theme()
                {
                    ShortInformation = "Running events", 
                    Subthemes = null,
                    ThemeName = "Running"
                        
                },
                new Theme()
                {
                    ShortInformation = "Cycling races", 
                    Subthemes = null,
                    ThemeName = "Cycling"
                },
                new Theme()
                {
                    ShortInformation = "Swimming competitions", 
                    Subthemes = null,
                    ThemeName = "Swimming"
                },
                new Theme()
                {
                    ShortInformation = "Football (Soccer) tournaments", 
                    Subthemes = null,
                    ThemeName = "Football"
                },
                new Theme()
                {
                    ShortInformation = "Tennis championships", 
                    Subthemes = null,
                    ThemeName = "Tennis"
                },
                new Theme()
                {
                    ShortInformation = "Basketball games", 
                    Subthemes = null,
                    ThemeName = "Basketball"
                },
                new Theme()
                {
                    ShortInformation = "Fitness activities", 
                    Subthemes = null,
                    ThemeName = "Fitness"
                },
                new Theme()
                {
                    ShortInformation = "Yoga classes", 
                    Subthemes = null,
                    ThemeName = "Yoga"
                },
                new Theme()
                {
                    ShortInformation = "Martial arts training", 
                    Subthemes = null,
                    ThemeName = "Martial Arts"
                },
                new Theme()
                {
                    ShortInformation = "Team sports competitions", 
                    Subthemes = null, 
                    ThemeName = "Team Sports"
                },
                new Theme()
                {
                    ShortInformation = "Individual sports activities", 
                    Subthemes = null,
                    ThemeName = "Individual Sports"
                },
                new Theme()
                {
                    ShortInformation = "Spectator sports events", 
                    Subthemes = null,
                    ThemeName = "Spectator Sports"
                }
            }

        };
        
        Theme headTheme4 = new Theme()
        {
            ShortInformation = "Projects related to fiber optic network rollout",
            ThemeName = "Fiberklaar",
            IsHeadTheme = true,
            Subthemes = new List<Theme>()
            {
                new Theme()
                {
                    ShortInformation = "Fiber optic infrastructure deployment", 
                    Subthemes = null,    
                    ThemeName = "Infrastructure"
                },
                new Theme()
                {
                    ShortInformation = "Fiber optic internet service availability", 
                    Subthemes = null,
                    ThemeName = "Internet Service"
                },
                new Theme()
                {
                    ShortInformation = "Benefits of fiber optic technology", 
                    Subthemes = null,
                    ThemeName = "Benefits"
                },
                new Theme()
                {
                    ShortInformation = "Fiber optic network expansion plans", 
                    Subthemes = null,
                    ThemeName = "Expansion Plans"
                },
                new Theme()
                {
                    ShortInformation = "Comparison of fiber optic with other internet technologies", 
                    Subthemes = null,
                    ThemeName = "Comparison"
                },
                new Theme()
                {
                    ShortInformation = "Customer support for fiber optic connections", 
                    Subthemes = null,
                    ThemeName = "Customer Support"
                },
                new Theme()
                {
                    ShortInformation = "Smart home integration with fiber optic networks", 
                    Subthemes = null,
                    ThemeName = "Smart Home Integration"
                },
                new Theme()
                {
                    ShortInformation = "Business applications of fiber optic networks", 
                    Subthemes = null,
                    ThemeName = "Business Applications"
                },
                new Theme()
                {
                    ShortInformation = "Future-proofing with fiber optic technology", 
                    Subthemes = null,
                    ThemeName = "Future-Proofing"
                },
                new Theme()
                {
                    ShortInformation = "Cost considerations of fiber optic installations", 
                    Subthemes = null,
                    ThemeName = "Cost Considerations"
                },
                new Theme()
                {
                    ShortInformation = "Environmental impact of fiber optic networks", 
                    Subthemes = null,
                    ThemeName = "Environmental Impact"
                },
                new Theme()
                {
                    ShortInformation = "Regulations and permits for fiber optic deployments", 
                    Subthemes = null,
                    ThemeName = "Regulations"
                }
            }
        };

        Theme headTheme5 = new Theme
        {
            ThemeName = "Opvang instellingen",
            ShortInformation = "Projecten gerelateerd aan kinderopvanginstellingen",
            IsHeadTheme = true,
            Subthemes = new List<Theme>()
            {
                new Theme
                {
                    ThemeName = "Daycare centers", 
                    Subthemes = null, 
                    ShortInformation = "Kinderdagverblijven"
                }, 
                new Theme
                {
                    ThemeName = "After-school programs", 
                    Subthemes = null, 
                    ShortInformation = "Buitenschoolse opvang"
                },
                new Theme
                {
                    ThemeName = "Child development and education", 
                    Subthemes = null, 
                    ShortInformation = "Kinderontwikkeling en educatie"
                },
                new Theme
                {
                    ThemeName = "Support for parents", 
                    Subthemes = null, 
                    ShortInformation = "Ondersteuning voor ouders"
                }, 
                new Theme
                {
                    ThemeName = "Regulations and licensing", 
                    Subthemes = null, 
                    ShortInformation = "Regelgeving en vergunningen"
                }, 
                new Theme
                {
                    ThemeName = "Special needs care", 
                    Subthemes = null, 
                    ShortInformation = "Zorg voor kinderen met speciale behoeften"
                }, 
                new Theme
                {
                    ThemeName = "Accessibility and inclusion", 
                    Subthemes = null, 
                    ShortInformation = "Toegankelijkheid en inclusie"
                }, 
                new Theme
                {
                    ThemeName = "Nutritional programs", 
                    Subthemes = null, 
                    ShortInformation = "Voedingsprogramma's voor kinderen"
                }, 
                new Theme
                {
                    ThemeName = "Staff training", 
                    Subthemes = null, 
                    ShortInformation = "Opleidingen en kwalificaties personeel"
                }, 
                new Theme
                {
                    ThemeName = "Safety & Security", 
                    Subthemes = null, 
                    ShortInformation = "Veiligheidsmaatregelen"
                }, 
                new Theme
                {
                    ThemeName = "Funding", 
                    Subthemes = null, 
                    ShortInformation = "Financiering en subsidies"
                }, 
                new Theme
                {
                    ThemeName = "Collaboration", 
                    Subthemes = null, 
                    ShortInformation = "Samenwerking met scholen en andere instellingen"
                }, 
            }
        };

        Theme headTheme6 = new Theme
        {
            ThemeName = "Psychologen",
            ShortInformation = "Projecten gerelateerd aan psychologie en geestelijke gezondheidszorg", 
            IsHeadTheme = true,
            Subthemes = new List<Theme>()
            {
                new Theme
                {
                    ThemeName = "Adult psychology", 
                    Subthemes = null, 
                    ShortInformation = "Psychologie voor volwassenen"
                }, 
                new Theme
                {
                    ThemeName = "Child psychology", 
                    Subthemes = null, 
                    ShortInformation = "Kinder- en jeugdpsychologie"
                }, 
                new Theme
                {
                    ThemeName = "CBT", 
                    Subthemes = null, 
                    ShortInformation = "Cognitieve gedragstherapie"
                }, 
                new Theme
                {
                    ThemeName = "Psychodynamic therapy", 
                    Subthemes = null, 
                    ShortInformation = "Psychodynamische therapie"
                }, 
                new Theme
                {
                    ThemeName = "Family therapy", 
                    Subthemes = null, 
                    ShortInformation = "Gezinstherapie"
                }, 
                new Theme
                {
                    ThemeName = "Group therapy", 
                    Subthemes = null, 
                    ShortInformation = "Groepstherapie"
                }, 
                new Theme
                {
                    ThemeName = "Stress management", 
                    Subthemes = null, 
                    ShortInformation = "Stress management"
                }, 
                new Theme
                {
                    ThemeName = "Depression & Anxiety", 
                    Subthemes = null, 
                    ShortInformation = "Depressie- en angstigheidsbehandeling"
                }, 
                new Theme
                {
                    ThemeName = "Trauma therapy", 
                    Subthemes = null, 
                    ShortInformation = "Trauma therapie"
                }, 
                new Theme
                {
                    ThemeName = "Mental health awareness",
                    Subthemes = null, 
                    ShortInformation = "Bewustwording en voorlichting geestelijke gezondheid"
                }, 
                new Theme
                {
                    ThemeName = "Ethics in psychology", 
                    Subthemes = null, 
                    ShortInformation = "Ethiek in de psychologie"
                }, 
                new Theme
                {
                    ThemeName = "Accessibility", 
                    Subthemes = null, 
                    ShortInformation = "Toegankelijkheid van geestelijke gezondheidszorg"
                }, 
            }
        };
        
        phygitalDbContext.Themes.Add(headTheme);
        phygitalDbContext.Themes.Add(headTheme2);
        phygitalDbContext.Themes.Add(headTheme3);
        phygitalDbContext.Themes.Add(headTheme4);
        phygitalDbContext.Themes.Add(headTheme5);
        phygitalDbContext.Themes.Add(headTheme6);

        foreach (var subtheme in headTheme.Subthemes)
        {
            phygitalDbContext.Themes.Add(subtheme);
        }
        foreach (var subtheme in headTheme2.Subthemes)
        {
            phygitalDbContext.Themes.Add(subtheme);
        }
        foreach (var subtheme in headTheme3.Subthemes)
        {
            phygitalDbContext.Themes.Add(subtheme);
        }
        foreach (var subtheme in headTheme4.Subthemes)
        {
            phygitalDbContext.Themes.Add(subtheme);
        }
        foreach (var subtheme in headTheme5.Subthemes)
        {
            phygitalDbContext.Themes.Add(subtheme);
        }
        foreach (var subtheme in headTheme6.Subthemes)
        {
            phygitalDbContext.Themes.Add(subtheme);
        }

        phygitalDbContext.SaveChanges();

        #endregion

        #region Question_conditioneel

        QuestionWithOption questionWithOption1Conditioneel = new QuestionWithOption
        {
            QuestionText = "Jij gaf aan dat je waarschijnlijk niet zal gaan stemmen. Om welke reden(en) zeg je dit?",
            QuestionType = QuestionType.Multiple,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption()
                {
                    TextAnswer = "Ik heb geen interesse",
                    //Question = questionWithOption1Conditioneel
                },
                new AnswerOption()
                {
                    TextAnswer = "Ik heb geen tijd om te gaan stemmen",
                    //Question = questionWithOption1Conditioneel
                },
                new AnswerOption()
                {
                    TextAnswer = "Ik kan niet naar het stemkantoor gaan",
                    //Question = questionWithOption1Conditioneel
                },
                new AnswerOption()
                {
                    TextAnswer = "Ik denk niet dat mijn stem een verschil zal uitmaken\n",
                    //Question = questionWithOption1Conditioneel
                },
                new AnswerOption()
                {
                    TextAnswer = "Ik heb geen idee voor wie ik zou moeten stemmen",
                    //Question = questionWithOption1Conditioneel
                }
            }
        };
        QuestionWithOption questionWithOption2Conditioneel = new QuestionWithOption
        {
            QuestionText = "Wat zou jou (meer) zin geven om te gaan stemmen?",
            QuestionType = QuestionType.Multiple,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption()
                {
                    TextAnswer = "Kunnen gaan stemmen op een toffere locatie",
                    //Question = questionWithOption2Conditioneel
                },
                new AnswerOption()
                {
                    TextAnswer = "Online, van thuis uit kunnen stemmen",
                    //Question = questionWithOption2Conditioneel
                },
                new AnswerOption()
                {
                    TextAnswer = "Betere inhoudelijke voorstellen van de politieke partijen",
                    //Question = questionWithOption2Conditioneel
                },
                new AnswerOption()
                {
                    TextAnswer = "Meer aandacht voor jeugd in de programma’s van de partijen",
                    //Question = questionWithOption2Conditioneel
                },
                new AnswerOption()
                {
                    TextAnswer = "Beter weten of mijn stem echt impact heeft",
                    //Question = questionWithOption2Conditioneel
                }
            }
        };
        InformationContent informationContentConditional = new InformationContent
        {
            Title="Hello",
            TextInformation = "Dit is het eerste conditionele punt"
        };

        InformationContent informationContentConditional2 = new InformationContent
        {
            Title="Hello",
            TextInformation = "Dit is het tweede conditionele punt"
        };

        InformationContent informationContentConditional3 = new InformationContent
        {
            Title="Hello",
            TextInformation = "Dit is een conditioneel punt"
        };

        phygitalDbContext.QuestionWithOptions.AddRange(questionWithOption1Conditioneel,
            questionWithOption2Conditioneel);
        phygitalDbContext.InformationContent.AddRange(informationContentConditional, informationContentConditional2,
            informationContentConditional3);
        phygitalDbContext.Questions.AddRange(questionWithOption1Conditioneel, questionWithOption2Conditioneel);
        phygitalDbContext.SaveChanges();

        #endregion
        
        #region ConditionalStep

        Step conditioneelStep1 = new Step
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Stem redenen"),
            Content = questionWithOption1Conditioneel,
            StepState = State.Open,
            IsConditioneel = true,
            NextStep = null,
            Name = "Reden van stemmen"
        };
        Step conditioneelStep3 = new Step
        {
            Theme = phygitalDbContext.Themes.FirstOrDefault(th => th.ThemeName == "Vrije tijd"),
            Content = informationContentConditional,
            StepState = State.Open,
            IsConditioneel = true,
            NextStep = null,
            Name = "Conditionele step 3"
        };
        Step conditioneelStep4 = new Step
        {
            Theme = phygitalDbContext.Themes.FirstOrDefault(th => th.ThemeName == "Vrije tijd"),
            Content = informationContentConditional2,
            StepState = State.Open,
            IsConditioneel = true,
            NextStep = null,
            Name = "Conditionele step 4"
        };
        Step conditioneelStep5 = new Step
        {
            Theme = phygitalDbContext.Themes.FirstOrDefault(th => th.ThemeName == "Vrije tijd"),
            Content = informationContentConditional3,
            StepState = State.Open,
            IsConditioneel = true,
            NextStep = null,
            Name = "Conditionele step 5"
        };

        #endregion

        #region ConditionalPoint

        ConditionalPoint kiesintentiesConditionalPoint = new ConditionalPoint()
        {
            ConditionalPointName = "Kiesintenties interesse",
            ConditionalStep = conditioneelStep1
        };
        phygitalDbContext.ConditionalPoints.Add(kiesintentiesConditionalPoint);
        phygitalDbContext.SaveChanges();

        #endregion

        #region QuestionWithOption_Single
        
        QuestionWithOption questionWithOption1Single = new QuestionWithOption
        {
            QuestionText =
                "Als jij de begroting van je stad of gemeente zou opmaken, waar zou je dan in de komende jaren vooral op inzetten?",
            QuestionType = QuestionType.Single,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption()
                {
                    TextAnswer = "Natuur en ecologie"
                },
                new AnswerOption()
                {
                    TextAnswer = "Vrije tijd, sport, cultuur",
                    //Question = questionWithOption1Single
                },
                new AnswerOption()
                {
                    TextAnswer = "Huisvesting",
                    //Question = questionWithOption1Single
                },
                new AnswerOption()
                {
                    TextAnswer = "Onderwijs en kinderopvang",
                    //Question = questionWithOption1Single
                },
                new AnswerOption()
                {
                    TextAnswer = "Gezondheidszorg en welzijn",
                    //Question = questionWithOption1Single
                }
            }
        };

        QuestionWithOption questionWithOption2Single = new QuestionWithOption
        {
            QuestionText =
                "\"Er moet meer geïnvesteerd worden in overdekte fietsstallingen aan de bushaltes in onze gemeente.\" Wat vind jij van dit voorstel?",
            QuestionType = QuestionType.Single,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption()
                {
                    TextAnswer = "Eens",
                    //Question = questionWithOption2Single
                },
                new AnswerOption()
                {
                    TextAnswer = "Oneens",
                    //Question = questionWithOption2Single
                }
            }
        };
        QuestionWithOption questionWithOption3Single = new QuestionWithOption
        {
            QuestionText = "Waarop wil jij dat de focus wordt gelegd in het nieuwe stadspark?",
            QuestionType = QuestionType.Single,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption()
                {
                    TextAnswer = "Sportinfrastructuur",
                    //Question = questionWithOption3Single
                },
                new AnswerOption()
                {
                    TextAnswer = "Speeltuin voor kinderen",
                    //Question = questionWithOption3Single
                },
                new AnswerOption()
                {
                    TextAnswer = "Zitbanken en picknickplaatsen",
                    //Question = questionWithOption3Single
                },
                new AnswerOption()
                {
                    TextAnswer = "Ruimte voor kleine evenementen",
                    //Question = questionWithOption3Single
                },
                new AnswerOption()
                {
                    TextAnswer = "Drank- en eetmogelijkheden",
                    //Question = questionWithOption3Single
                }
            }
        };
        QuestionWithOption questionWithOption4Single = new QuestionWithOption
        {
            QuestionText = "Hoe sta jij tegenover deze stelling? “Mijn stad moet meer investeren in fietspaden“",
            QuestionType = QuestionType.Single,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption()
                {
                    TextAnswer = "Akkoord",
                    //Question = questionWithOption4Single
                },
                new AnswerOption()
                {
                    TextAnswer = "Niet akkoord",
                    //Question = questionWithOption4Single
                }
            }
        };
        QuestionWithOption questionWithOption5Single = new QuestionWithOption
        {
            QuestionText = "Om ons allemaal veilig en vlot te verplaatsen, moet er in jouw stad of gemeente vooral meer aandacht gaan naar:",
            QuestionType = QuestionType.Single,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption()
                {
                    TextAnswer = "Verplaatsingen met de fiets",
                    //Question = questionWithOption5Single
                },
                new AnswerOption()
                {
                    TextAnswer = "Verplaatsingen met de auto/moto",
                    //Question = questionWithOption5Single
                },
                new AnswerOption()
                {
                    TextAnswer = "Verplaatsingen te voet",
                    //Question = questionWithOption5Single
                },
                new AnswerOption()
                {
                    TextAnswer = "Deelmobiliteit",
                    //Question = questionWithOption5Single
                },
                new AnswerOption()
                {
                    TextAnswer = "Openbaar vervoer",
                    //Question = questionWithOption5Single
                }
            }
        };
        QuestionWithOption questionWithOption6Single = new QuestionWithOption
        {
            QuestionText = "Wat vind jij van het idee om alle leerlingen van de scholen in onze stad een gratis fiets aan te bieden?\n",
            QuestionType = QuestionType.Single,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption()
                {
                    TextAnswer = "Goed idee",
                    //Question = questionWithOption6Single
                },
                new AnswerOption()
                {
                    TextAnswer = "Slecht idee",
                    //Question = questionWithOption6Single
                }
            }
        };
        
        phygitalDbContext.Questions.AddRange(questionWithOption1Single, questionWithOption2Single,
            questionWithOption3Single, questionWithOption4Single, questionWithOption5Single, questionWithOption6Single);
        phygitalDbContext.SaveChanges();

        #endregion

        #region QuestionWithOption_Multi

        QuestionWithOption questionWithOption1Multiple = new QuestionWithOption
        {
            QuestionText = "Wat zou jou helpen om een keuze te maken tussen de verschillende partijen?",
            QuestionType = QuestionType.Multiple,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption()
                {
                    TextAnswer = "Meer lessen op school rond de partijprogramma’s",
                    //Question = questionWithOption1Multiple
                },
                new AnswerOption()
                {
                    TextAnswer = "Activiteiten in mijn jeugdclub, sportclub… rond de verkiezingen",
                    //Question = questionWithOption1Multiple
                },
                new AnswerOption()
                {
                    TextAnswer = "Een bezoek van de partijen aan mijn school, jeugd/sportclub, …",
                    //Question = questionWithOption1Multiple
                },
                new AnswerOption()
                {
                    TextAnswer = "Een gesprek met mijn ouders rond de gemeentepolitiek",
                    //Question = questionWithOption1Multiple
                },
                new AnswerOption()
                {
                    TextAnswer = "Een debat georganiseerd door een jeugdhuis met de verschillende partijen",
                    //Question = questionWithOption1Multiple
                }
            }
        };
        QuestionWithOption questionWithOption2Multiple = new QuestionWithOption
        {
            QuestionText = "Welke sportactiviteit(en) zou jij graag in je eigen stad of gemeente kunnen beoefenen?",
            QuestionType = QuestionType.Multiple,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption()
                {
                    TextAnswer = "Tennis",
                    //Question = questionWithOption2Multiple
                },
                new AnswerOption()
                {
                    TextAnswer = "Hockey",
                    //Question = questionWithOption2Multiple
                },
                new AnswerOption()
                {
                    TextAnswer = "Padel",
                    //Question = questionWithOption2Multiple
                },
                new AnswerOption()
                {
                    TextAnswer = "Voetbal",
                    //Question = questionWithOption2Multiple
                },
                new AnswerOption()
                {
                    TextAnswer = "Fitness",
                    //Question = questionWithOption2Multiple
                }
            }
        };
        QuestionWithOption questionWithOption3Multiple = new QuestionWithOption
        {
            QuestionText = "Aan welke van deze activiteiten zou jij meedoen, om mee te wegen op het beleid van jouw stad of gemeente?",
            QuestionType = QuestionType.Multiple,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption()
                {
                    TextAnswer = "Deelnemen aan gespreksavonden met schepenen en burgemeester",
                    //Question = questionWithOption3Multiple
                },
                new AnswerOption()
                {
                    TextAnswer = "Bijwonen van een gemeenteraad",
                    //Question = questionWithOption3Multiple
                },
                new AnswerOption()
                {
                    TextAnswer = "Deelnemen aan een survey uitgestuurd door de stad of gemeente",
                    //Question = questionWithOption3Multiple
                },
                new AnswerOption()
                {
                    TextAnswer = "Een overleg waarbij ik onderwerpen kan aandragen die voor jongeren belangrijk zijn",
                    //Question = questionWithOption3Multiple
                },
                new AnswerOption()
                {
                    TextAnswer = "Mee brainstormen over oplossingen voor problemen waar jongeren mee worstelen",
                    //Question = questionWithOption3Multiple
                }
            }
        };
        
        phygitalDbContext.QuestionWithOptions.AddRange(questionWithOption1Multiple, questionWithOption2Multiple,
            questionWithOption3Multiple);
        phygitalDbContext.Questions.AddRange(questionWithOption1Multiple, questionWithOption2Multiple,
            questionWithOption3Multiple);
        phygitalDbContext.SaveChanges();

        #endregion

        #region QuestionWithOption_Range

        QuestionWithOption questionWithOption1Range = new QuestionWithOption
        {
            QuestionText = "Ben jij van plan om te gaan stemmen bij de aankomende lokale verkiezingen?",
            QuestionType = QuestionType.Range,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption()
                {
                    TextAnswer = "Zeker niet",
                    //Question = questionWithOption1Range
                },
                new AnswerOption()
                {
                    TextAnswer = "Eerder niet",
                    //Question = questionWithOption1Range,
                    ConditionalPoint = kiesintentiesConditionalPoint
                },
                new AnswerOption()
                {
                    TextAnswer = "Geen mening",
                    //Question = questionWithOption1Range
                },
                new AnswerOption()
                {
                    TextAnswer = "Eerder wel",
                    //Question = questionWithOption1Range
                },
                new AnswerOption()
                {
                    TextAnswer = "Zeker wel",
                    //Question = questionWithOption1Range
                }
            }
        };

        QuestionWithOption questionWithOption2Range = new QuestionWithOption
        {
            QuestionText = "Voel jij je betrokken bij het beleid dat wordt uitgestippeld door je gemeente of stad?\n",
            QuestionType = QuestionType.Range,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption()
                {
                    TextAnswer = "Ik voel me weinig tot niet betrokken",
                    //Question = questionWithOption2Range
                },
                new AnswerOption()
                {
                    TextAnswer = "Ik voel me eerder niet betrokken",
                    //Question = questionWithOption2Range
                },
                new AnswerOption()
                {
                    TextAnswer = "Ik weet het nog niet",
                    //Question = questionWithOption2Range
                },
                new AnswerOption()
                {
                    TextAnswer = "Ik voel me eerder betrokken",
                    //Question = questionWithOption2Range
                },
                new AnswerOption()
                {
                    TextAnswer = "Ik voel me (zeer) betrokken",
                    //Question = questionWithOption2Range
                }
            }
        };
        QuestionWithOption questionWithOption3Range = new QuestionWithOption
        {
            QuestionText = "In hoeverre ben jij tevreden met het vrijetijdsaanbod in jouw stad of gemeente?",
            QuestionType = QuestionType.Range,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption()
                {
                    TextAnswer = "Zeer ontevreden",
                    //Question = questionWithOption3Range
                },
                new AnswerOption()
                {
                    TextAnswer = "Ontevreden",
                    //Question = questionWithOption3Range
                },
                new AnswerOption()
                {
                    TextAnswer = "Geen mening",
                    //Question = questionWithOption3Range
                },
                new AnswerOption()
                {
                    TextAnswer = "Tevreden",
                    //Question = questionWithOption3Range
                },
                new AnswerOption()
                {
                    TextAnswer = "Zeer tevreden",
                    //Question = questionWithOption3Range
                }
            }
        };
        QuestionWithOption questionWithOption4Range = new QuestionWithOption
        {
            QuestionText =
                "In welke mate ben jij het eens met de volgende stelling: “Mijn stad of gemeente doet voldoende om betaalbare huisvesting mogelijk te maken voor iedereen.”",
            QuestionType = QuestionType.Range,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption()
                {
                    TextAnswer = "Helemaal oneens",
                    //Question = questionWithOption4Range
                },
                new AnswerOption()
                {
                    TextAnswer = "Oneens",
                    //Question = questionWithOption4Range
                },
                new AnswerOption()
                {
                    TextAnswer = "Geen mening",
                    //Question = questionWithOption4Range
                },
                new AnswerOption()
                {
                    TextAnswer = "Eens",
                    //Question = questionWithOption4Range
                },
                new AnswerOption()
                {
                    TextAnswer = "Helemaal eens",
                    //Question = questionWithOption4Range
                }
            }
        };
        QuestionWithOption questionWithOption5Range = new QuestionWithOption
        {
            QuestionText = "In welke mate kun jij je vinden in het voorstel om de straatlichten in onze gemeente te doven tussen 23u en 5u?",
            QuestionType = QuestionType.Range,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption()
                {
                    TextAnswer = "Ik sta hier volledig achter",
                    //Question = questionWithOption5Range
                },
                new AnswerOption()
                {
                    TextAnswer = "Ik sta hier eerder achter",
                    //Question = questionWithOption5Range
                },
                new AnswerOption()
                {
                    TextAnswer = "Ik weet het nog niet",
                    //Question = questionWithOption5Range
                },
                new AnswerOption()
                {
                    TextAnswer = "Ik sta hier eerder niet achter",
                    //Question = questionWithOption5Range
                },
                new AnswerOption()
                {
                    TextAnswer = "Ik sta hier helemaal niet achter",
                    //Question = questionWithOption5Range
                }
            }
        };

        phygitalDbContext.QuestionWithOptions.AddRange(questionWithOption1Range, questionWithOption2Range,
            questionWithOption3Range, questionWithOption4Range, questionWithOption5Range);
        phygitalDbContext.Questions.AddRange(questionWithOption1Range, questionWithOption2Range,
            questionWithOption3Range, questionWithOption4Range, questionWithOption5Range);
        phygitalDbContext.SaveChanges();

        #endregion

        #region QuestionOpen

        QuestionOpen questionOpen1 = new QuestionOpen
        {
            QuestionText = "Je bent schepen van onderwijs voor een dag: waar zet je dan vooral op in?",
            QuestionType = QuestionType.Open
        };
        QuestionOpen questionOpen2 = new QuestionOpen
        {
            QuestionText = "Als je één ding mag wensen voor het nieuwe stadspark, wat zou jouw droomstadspark dan zeker bevatten?",
            QuestionType = QuestionType.Open
        };
        phygitalDbContext.OpenQuestions.AddRange(questionOpen1, questionOpen2);
        phygitalDbContext.Questions.AddRange(questionOpen1, questionOpen2);

        #endregion

        #region Information

        
        ICollection<InformationContent> informations = new List<InformationContent>()
        {
            new InformationContent()
            {
                TextInformation =
                    "Dear participant," +
                    "\n\nThank you very much for participating in the \"phygytaltool\" survey! Your valuable input will help us gain insight into this subject and improve our future activities and services." +
                    "\n\nYour participation contributes to the understanding of the intersection between physical and digital tools and will help us to continue innovating in our field." +
                    "\n\nOnce again, thank you for taking the time and effort to complete this survey. We greatly appreciate your contribution." +
                    "\n\nBest regards," +
                    "\n\nPhygtalTool",
                ObjectName = "audio/downfall-3-208028.mp3", 
                ContentType = "audio/mpeg"
            }
        };
        phygitalDbContext.InformationContent.AddRange(informations);
        phygitalDbContext.Content.AddRange(informations);
        phygitalDbContext.SaveChanges();


        Step infoStep2 = new Step()
        {
            Content = new InformationContent()
            {
                Title = "Blanco",
                TextInformation =
                    "Er bestaat soms een misverstand dat blanco stemmen automatisch naar de grootste partij gaat. " +
                    "In werkelijkheid worden blanco stemmen niet toegewezen aan enige kandidaat of partij en tellen" +
                    " ze niet mee in de uitslag. Ze worden simpelweg beschouwd als ongeldige stemmen en hebben geen invloed" +
                    " op de verkiezingsresultaten.",
                ObjectName = "",
                ContentType = ""
            },
            IsConditioneel = false,
            Name = "remove step",
            StepState = State.Open,
            Theme = headTheme3
        };
        Step infoStep = new Step()
        {
            Content = informations.First(),
            IsConditioneel = false,
            Name = "remove step",
            StepState = State.Open,
            Theme = headTheme3,
            NextStep = infoStep2
        };

        phygitalDbContext.Steps.Add(infoStep);
        #endregion

        #region Step

        Step step16 = new Step()
        {
            Theme = phygitalDbContext.Themes.FirstOrDefault(th => th.ThemeName == "Vrije tijd"),
            Content = questionOpen2,
            StepState = State.Open,
            NextStep = infoStep,
            IsConditioneel = false,
            Name = "Droomstadspark"
        };

        Step step15 = new Step()
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Onderwijs"),
            Content = questionOpen1,
            StepState = State.Open,
            NextStep = step16,
            IsConditioneel = false,
            Name = "Schepen voor een dag"
        };
        Step step14 = new Step()
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Energie"),
            Content = questionWithOption5Range,
            StepState = State.Open,
            NextStep = step15,
            IsConditioneel = false,
            Name = "Straatlichten doven"
        };
        Step step13 = new Step()
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Huisvesting"),
            Content = questionWithOption4Range,
            NextStep = step14,
            IsConditioneel = false,
            StepState = State.Open,
            Name = "Betaalbare huisvesting"
        };
        Step step12 = new Step()
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Vrije tijd"),
            Content = questionWithOption3Range,
            NextStep = step13,
            StepState = State.Open,
            IsConditioneel = false,
            Name = "Vrijetijdsaanbod"
        };

        Step step11 = new Step()
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Lokale betrokkenheid"),
            Content = questionWithOption2Range,
            NextStep = step12,
            StepState = State.Open,
            IsConditioneel = false,
            Name = "Betrokkenheid bij beleid"
        };
        Step step10 = new Step()
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Stem redenen"),
            Content = questionWithOption1Range,
            StepState = State.Open,
            NextStep = step11,
            IsConditioneel = false,
            Name = "Plannen om te gaan stemmen"
        };
        Step step9 = new Step()
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Beleidsparticipatie"),
            Content = questionWithOption3Multiple,
            StepState = State.Open,
            NextStep = step10,
            IsConditioneel = false,
            Name = "Inspraak beleidsactiviteiten"
        };
        Step step8 = new Step()
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Sport"),
            Content = questionWithOption2Multiple,
            StepState = State.Open,
            NextStep = step9,
            IsConditioneel = false,
            Name = "Sportactiviteiten"
        };
        Step step7 = new Step()
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Kiesintenties"),
            Content = questionWithOption1Multiple,
            StepState = State.Open,
            NextStep = step8,
            IsConditioneel = false,
            Name = "Kiezen tussen partijen"
        };
        Step step6 = new Step()
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Mobiliteit"),
            Content = questionWithOption6Single,
            StepState = State.Open,
            NextStep = step7,
            IsConditioneel = false,
            Name = "Gratis fiets voor studenten"
        };
        Step step5 = new Step()
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Mobiliteit"),
            Content = questionWithOption5Single,
            StepState = State.Open,
            NextStep = step6,
            IsConditioneel = false,
            Name = "Meer aandacht voor veilig verkeer"
        };
        Step step4 = new Step()
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Mobiliteit"),
            Content = questionWithOption4Single,
            StepState = State.Open,
            NextStep = step5,
            IsConditioneel = false,
            Name = "Investeren fietspaden"
        };
        Step step3 = new Step()
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Vrije tijd"),
            Content = questionWithOption3Single,
            StepState = State.Open,
            NextStep = step4,
            IsConditioneel = false,
            Name = "Focus stadspark"
        };
        Step step2 = new Step()
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Mobiliteit"),
            Content = questionWithOption2Single,
            StepState = State.Open,
            NextStep = step3,
            IsConditioneel = false,
            Name = "Overdekte fietsstallingen aan bushalte"
        };
        Step step1 = new Step()
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Beleidsparticipatie"),
            Content = questionWithOption1Single,
            StepState = State.Open,
            NextStep = step2,
            IsConditioneel = false,
            Name = "Investeringen in begroting"
        };
        Step conditioneelStep2 = new Step
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Stem redenen"),
            Content = questionWithOption2Conditioneel,
            StepState = State.Open,
            NextStep = step11,
            IsConditioneel = true,
            Name = "Meer zin in stemmen"
        };
        phygitalDbContext.Steps.AddRange(step1, step2, step3, step4, step5, step6, step7, step8, step8, step9, step10,
            step11, step12, step13, step14, step15, step16, conditioneelStep2, conditioneelStep3, conditioneelStep1,
            conditioneelStep4, conditioneelStep5);
        conditioneelStep1.NextStep = step11;
        phygitalDbContext.Steps.Update(conditioneelStep1);
        phygitalDbContext.SaveChanges();

        #endregion

        #region Flow

        Flow lokaleVerkiezingen = new Flow
        {
            FlowName = "Lokale verkiezingen",
            IsLinear = true,
            State = State.Open,
            FirstStep = step1,
            Steps = new List<Step>()
            {
                infoStep2,
                infoStep,
                step1, step2, step3, step4, step5, step6, step7, step8, step9, step10, step11, step12, step13,
                step14, step15, step16, conditioneelStep2, conditioneelStep1, conditioneelStep3, conditioneelStep4,
                conditioneelStep5
            }
        };
        Flow lokaleVerkiezingen1 = new Flow
        {
            FlowName = "Lokale verkiezingen Vlaanderen",
            IsLinear = true,
            State = State.Open,
            Steps = new List<Step>()
        };
        Flow lokaleVerkiezingen2 = new Flow
        {
            FlowName = "Lokale verkiezingen Wallonië",
            IsLinear = false,
            State = State.Open,
            Steps = new List<Step>()
        };
        Flow lokaleVerkiezingen3 = new Flow
        {
            FlowName = "Nationale verkiezingen",
            IsLinear = true,
            State = State.Open,
            Steps = new List<Step>()
        };
        Flow lokaleVerkiezingen4 = new Flow
        {
            FlowName = "Europese verkiezingen",
            IsLinear = true,
            State = State.Open,
            Steps = new List<Step>()
        };
        Flow lokaleVerkiezingen5 = new Flow
        {
            FlowName = "Openen van een rekening",
            IsLinear = true,
            State = State.Open,
            Steps = new List<Step>()
        };
        Flow lokaleVerkiezingen6 = new Flow
        {
            FlowName = "Sluiten van een rekening",
            IsLinear = true,
            State = State.Open,
            Steps = new List<Step>()
        };
        Flow lokaleVerkiezingen7 = new Flow
        {
            FlowName = "Het beheren van meerdere rekeningen",
            IsLinear = true,
            State = State.Open,
            Steps = new List<Step>()
        };
        Flow lokaleVerkiezingen8 = new Flow
        {
            FlowName = "Mogelijkheid tot bankieren",
            IsLinear = true,
            State = State.Open,
            Steps = new List<Step>()
        };
        
        phygitalDbContext.Flows.Add(lokaleVerkiezingen);
        phygitalDbContext.Flows.Add(lokaleVerkiezingen1);
        phygitalDbContext.Flows.Add(lokaleVerkiezingen2);
        phygitalDbContext.Flows.Add(lokaleVerkiezingen3);
        phygitalDbContext.Flows.Add(lokaleVerkiezingen4);
        phygitalDbContext.Flows.Add(lokaleVerkiezingen5);
        phygitalDbContext.Flows.Add(lokaleVerkiezingen6);
        phygitalDbContext.Flows.Add(lokaleVerkiezingen7);
        phygitalDbContext.Flows.Add(lokaleVerkiezingen8);
        phygitalDbContext.SaveChanges();
        #endregion
        
        #region Project

        Project project1 = new Project()
        {
            Name = "Verkiezingen in Vlaanderen",
            IsActive = true,
            Theme = headTheme,
            ProjectInformation = "Dit is een project over de verkiezingen in Vlaanderen",
            PrimaryColor = "Pale Turquoise",
            Font = "Arial",
            Flows = new List<Flow>()
            {
                lokaleVerkiezingen,
                lokaleVerkiezingen1,
                lokaleVerkiezingen2,
                lokaleVerkiezingen3,
                lokaleVerkiezingen4
            }
        };
        Project project2 = new Project()
        {
            Name = "De Nationale Banken",
            IsActive = true,
            Theme = headTheme2,
            Flows = new List<Flow>()
            {
                lokaleVerkiezingen5,
                lokaleVerkiezingen6,
                lokaleVerkiezingen7,
                lokaleVerkiezingen8
            }
        };
        Project project3 = new Project()
        {
            Name = "Sport in Antwerpen",
            IsActive = true,
            Theme = headTheme3
        };
        Project project4 = new Project()
        {
            Name = "Fiberklaar",
            IsActive = true,
            Theme = headTheme4
        };
        Project project5 = new Project()
        {
            Name = "Opvang instellingen",
            IsActive = true,
            Theme = headTheme5
        };
        Project project6 = new Project()
        {
            Name = "Psychologen",
            IsActive = true,
            Theme = headTheme6
        };
        phygitalDbContext.Projects.Add(project1);
        phygitalDbContext.Projects.Add(project2);
        phygitalDbContext.Projects.Add(project3);
        phygitalDbContext.Projects.Add(project4);
        phygitalDbContext.Projects.Add(project5);
        phygitalDbContext.Projects.Add(project6);
        phygitalDbContext.SaveChanges();

        #endregion

        #region Users

        ApplicationUser user1 = new ApplicationUser()
        {
            Id = "1",
            UserName = "User1",
            Email = ""
        };
        phygitalDbContext.ApplicationUsers.Add(user1);
        phygitalDbContext.SaveChanges();

        ApplicationUser applicationUser1 = new ApplicationUser()
        {
            FirstName = "Ilian",
            LastName = "Elst",
            UserName = "ilian.elst@student.kdg.be",
            Email = "ilian.elst@student.kdg.be",
            EmailConfirmed = true,
            PhoneNumber = "+32499766876",
            BirthDate = new DateOnly(2001, 07, 22),
            TwoFactorEnabled = false,
            MoreInfo = true,
            Sessions = new List<Session>()
            {
                new Session()
                {
                    Color = Color.Orange,
                    StartTime = new DateTime(2024, 05, 17, 13, 00, 00, DateTimeKind.Utc),
                    EndTime = new DateTime(2024, 05, 17, 13, 30, 00, DateTimeKind.Utc),
                    ExecusionTime = new TimeOnly(13, 00, 00),
                    Flow = new RunningFlow()
                    {
                        RunningFlowTime = new DateTime(2024, 04, 12, 12, 00, 12, DateTimeKind.Utc),
                        CurrentFlow = lokaleVerkiezingen4,
                        IsKiosk = true,
                        State = State.Open,
                        CreatedAttendantName = "Jaapje"
                    }
                }
            }
        };
        phygitalDbContext.ApplicationUsers.Add(applicationUser1);
        userManager.CreateAsync(applicationUser1, "Password1!").Wait();
        userManager.AddToRoleAsync(applicationUser1, CustomIdentityConstants.Application).Wait();

        ApplicationUser applicationUser2 = new ApplicationUser()
        {
            FirstName = "Matthew",
            LastName = "Vanloocke",
            UserName = "matthew.vanloocke@student.kdg.be",
            Email = "matthew.vanloocke@student.kdg.be",
            EmailConfirmed = true,
            PhoneNumber = "+31478568412",
            BirthDate = new DateOnly(2002, 04, 01),
            TwoFactorEnabled = false,
            MoreInfo = false,
            Sessions = new List<Session>()
            {
                new Session()
                {
                    Color = Color.Red,
                    StartTime = new DateTime(2024, 05, 17, 13, 00, 00, DateTimeKind.Utc),
                    EndTime = new DateTime(2024, 05, 17, 13, 30, 00, DateTimeKind.Utc),
                    ExecusionTime = new TimeOnly(13, 00, 00),
                    Flow = new RunningFlow()
                    {
                        RunningFlowTime = new DateTime(2024, 05, 17, 13, 01, 00, DateTimeKind.Utc),
                        CurrentFlow = lokaleVerkiezingen2,
                        IsKiosk = true,
                        State = State.Closed,
                        CreatedAttendantName = "Jaapje"
                    }
                },
                new Session()
                {
                    Color = Color.Green,
                    StartTime = new DateTime(2024, 05, 17, 13, 00, 00, DateTimeKind.Utc),
                    EndTime = new DateTime(2024, 05, 17, 13, 30, 00, DateTimeKind.Utc),
                    ExecusionTime = new TimeOnly(13, 00, 00),
                    Flow = new RunningFlow()
                    {
                        RunningFlowTime = new DateTime(2024, 02, 12, 13, 00, 00, DateTimeKind.Utc),
                        CurrentFlow = lokaleVerkiezingen3,
                        IsKiosk = false,
                        State = State.Closed,
                        CreatedAttendantName = "Jaapje"
                    }
                }
            }
        };
        phygitalDbContext.ApplicationUsers.Add(applicationUser2);
        userManager.CreateAsync(applicationUser2, "Password2!").Wait();
        userManager.AddToRoleAsync(applicationUser2, CustomIdentityConstants.Application).Wait();

        AttendentUser attendentUser1 = new AttendentUser()
        {
            FirstName = "Charlie",
            LastName = "Brooklyn",
            UserName = "charlie.brooklyn@vlaamseOverheid.be",
            Email = "charlie.brooklyn@vlaamseOverheid.be",
            EmailConfirmed = true,
            PhoneNumber = "+32478568412",
            BirthDate = new DateOnly(1995, 01, 13),
            TwoFactorEnabled = false,
            Organization = "Vlaamse Overheid",
            AssignedProject = project1
        };
        phygitalDbContext.AttendentUsers.Add(attendentUser1);
        userManager.CreateAsync(attendentUser1, "Password3!").Wait();
        userManager.AddToRoleAsync(attendentUser1, CustomIdentityConstants.Attendent).Wait();

        AttendentUser attendentUser2 = new AttendentUser()
        {
            FirstName = "Julien",
            LastName = "Vermoten",
            UserName = "julien.vermoten@vlaamseOverheid.be",
            Email = "julien.vermoten@vlaamseOverheid.be",
            EmailConfirmed = true,
            PhoneNumber = "+32466798521",
            BirthDate = new DateOnly(1997, 06, 24),
            TwoFactorEnabled = false,
            Organization = "Vlaamse Overheid",
            AssignedProject = project2
        };
        phygitalDbContext.AttendentUsers.Add(attendentUser2);
        userManager.CreateAsync(attendentUser2, "Password4!").Wait();
        userManager.AddToRoleAsync(attendentUser2, CustomIdentityConstants.Attendent).Wait();

        OrganizationUser organizationUser1 = new OrganizationUser()
        {
            FirstName = "Hanne",
            LastName = "Vanmoer",
            UserName = "hanne.vanmoer@vlaamseOverheid.be",
            Email = "hanne.vanmoer@vlaamseOverheid.be",
            EmailConfirmed = true,
            PhoneNumber = "+32499785264",
            BirthDate = new DateOnly(1978, 05, 16),
            TwoFactorEnabled = false,
            Organization = "Vlaamse Overheid",
            OwnedProjects = new List<Project>()
            {
                project1,
                project2,
                project5,
                project6
            }
        };
        phygitalDbContext.OrganisationUsers.Add(organizationUser1);
        userManager.CreateAsync(organizationUser1, "Password5!").Wait();
        userManager.AddToRoleAsync(organizationUser1, CustomIdentityConstants.Organization).Wait();

        OrganizationUser organizationUser2 = new OrganizationUser()
        {
            FirstName = "Charlotte",
            LastName = "Raats",
            UserName = "charlotte.raats@proximus.be",
            Email = "charlotte.raats@proximus.be",
            EmailConfirmed = true,
            PhoneNumber = "+32455712141",
            BirthDate = new DateOnly(1984, 09, 05),
            TwoFactorEnabled = false,
            Organization = "Proximus",
            OwnedProjects = new List<Project>()
            {
                project4
            }
        };
        phygitalDbContext.OrganisationUsers.Add(organizationUser2);
        userManager.CreateAsync(organizationUser2, "Password6!").Wait();
        userManager.AddToRoleAsync(organizationUser2, CustomIdentityConstants.Organization).Wait();

        #endregion
        
        #region Platform

        Platform platform1 = new Platform()
        {
            IsHead = false,
            PlatformName = "Vlaamse Overheid",
            CreationDate = new DateOnly(2024, 04, 23),
            OrganizationMaintainer = new List<OrganizationUser>()
            {
                organizationUser1
            },
            ProjectsAssigned = new List<Project>()
            {
                project1,
                project2,
                project5,
                project6
            }
        };

        Platform platform2 = new Platform()
        {
            IsHead = false,
            PlatformName = "Proximus",
            CreationDate = new DateOnly(2024, 04, 23),
            OrganizationMaintainer = new List<OrganizationUser>()
            {
                organizationUser2
            },
            ProjectsAssigned = new List<Project>()
            {
                project4
            }
        };

        Platform platform3 = new Platform()
        {
            IsHead = false,
            PlatformName = "Provincie Antwerpen",
            CreationDate = new DateOnly(2024, 04, 23),
            OrganizationMaintainer = new List<OrganizationUser>()
        };

        Platform platform4 = new Platform()
        {
            IsHead = false,
            PlatformName = "Provincie Oost-Vlaanderen",
            CreationDate = new DateOnly(2024, 04, 23),
            OrganizationMaintainer = new List<OrganizationUser>()
        };

        Platform platformHead = new Platform()
        {
            IsHead = true,
            PlatformName = "Phygital",
            CreationDate = new DateOnly(2024, 04, 23),
            SharingPlatforms = new List<Platform>()
            {
                platform1,
                platform2,
                platform3,
                platform4
            }
        };
        
        phygitalDbContext.Platforms.Add(platform1);
        phygitalDbContext.Platforms.Add(platform2);
        phygitalDbContext.Platforms.Add(platform3);
        phygitalDbContext.Platforms.Add(platform4);
        phygitalDbContext.Platforms.Add(platformHead);
        phygitalDbContext.SaveChanges();

        #endregion
        
        #region HeadOfPlatformUsers

        HeadOfPlatformUser headOfPlatformUser1 = new HeadOfPlatformUser()
        {
            FirstName = "Hanne",
            LastName = "Van Elsacker",
            UserName = "hanneVanElsacker@levuur.be",
            Email = "hanneVanElsacker@levuur.be",
            EmailConfirmed = true,
            PhoneNumber = "+32499785874",
            BirthDate = new DateOnly(1984, 11, 16),
            TwoFactorEnabled = false,
            ControlledPlatforms = new List<Platform>()
            {
                platform1,
                platform2,
                platform3,
                platform4,
                platformHead
            }
        };
        phygitalDbContext.HeadOfPlatformUsers.Add(headOfPlatformUser1);
        userManager.CreateAsync(headOfPlatformUser1, "Password7!").Wait();
        userManager.AddToRoleAsync(headOfPlatformUser1, CustomIdentityConstants.HeadOfPlatform).Wait();

        #endregion

        #region Comments

        Comment subComment1 = new Comment()
        {
            Text = "This is a comment to a comment",
            Likes = 5,
            User = applicationUser1
        };

        Comment headComment1 = new Comment()
        {
            Text = "This is a comment",
            User = applicationUser2,
            Likes = 0,
            SubComments = new List<Comment>()
            {
                subComment1
            }
        };
        
        Comment headComment2 = new Comment()
        {
            Text = "This is a third comment without a user.",
            Likes = 10
        };
        
        Comment headComment3 = new Comment()
        {
            Text = "This is a fourth comment with a image.",
            Likes = 10,
            User = applicationUser1,
            ObjectName = "image/test_comment_img.jpg"
        };

        phygitalDbContext.Comments.AddRange(headComment1, subComment1, headComment2, headComment3);
        phygitalDbContext.SaveChanges();
        Theme theme1 = phygitalDbContext.Themes.Include(t => t.Comments)
            .FirstOrDefault(th => th.ThemeName == "Vrije tijd");
        theme1.Comments.Add(headComment1);
        theme1.Comments.Add(headComment2);
        theme1.Comments.Add(headComment3);
        phygitalDbContext.SaveChanges();

        #endregion
        
        #region Session

        
        ICollection<AnonymousUser> anonymousUsers = new List<AnonymousUser>();
        for (int i = 0; i < 3; i++)
        {
            var anonymousUser = new AnonymousUser()
            {
                ActiveDate = new DateOnly(2024, 05, 21),
                Session = new Session()
                {
                    Color = (i == 0) ? Color.Blue : (i == 1) ? Color.Red : Color.Green,
                    StartTime = new DateTime(2024, 05, 17, 13, 00, 00, DateTimeKind.Utc),
                    EndTime = new DateTime(2024, 05, 17, 13, 30, 00, DateTimeKind.Utc),
                    ExecusionTime = new TimeOnly(13, 00, 00),
                    Flow = new RunningFlow()
                    {
                        RunningFlowTime = new DateTime(2024, 05, 17, 01, 00, 00, DateTimeKind.Utc),
                        CreatedAttendantName = attendentUser1.UserName,
                        IsKiosk = false,
                        CurrentFlow = lokaleVerkiezingen,
                        State = State.Closed
                    }
                }
            };
            anonymousUsers.Add(anonymousUser);
        }

        foreach (var anonymousUser in anonymousUsers)
        {
            phygitalDbContext.AnonymousUsers.Add(anonymousUser);
            phygitalDbContext.Sessions.Add(anonymousUser.Session);
        }

        phygitalDbContext.SaveChanges();

        #endregion

        #region UserAnswers

        UserAnswer userAnswer1 = new UserAnswer()
        {
            AnswerId = 17,
            Session = phygitalDbContext.Sessions.FirstOrDefault(s => s.Id == 1)
        };

        UserAnswer userAnswer2 = new UserAnswer()
        {
            AnswerId = 15,
            Session = phygitalDbContext.Sessions.FirstOrDefault(s => s.Id == 1)
        };

        UserAnswer userAnswer3 = new UserAnswer()
        {
            AnswerId = 21,
            Session = phygitalDbContext.Sessions.FirstOrDefault(s => s.Id == 2)
        };

        UserAnswer userAnswer4 = new UserAnswer()
        {
            AnswerId = 24,
            Session = phygitalDbContext.Sessions.FirstOrDefault(s => s.Id == 2)
        };

        UserAnswer userAnswer5 = new UserAnswer()
        {
            AnswerId = 27,
            Session = phygitalDbContext.Sessions.FirstOrDefault(s => s.Id == 3)
        };

        phygitalDbContext.UserAnswers.AddRange(userAnswer1, userAnswer2, userAnswer3, userAnswer4, userAnswer5);
        phygitalDbContext.SaveChanges();

        #endregion


        //Second case: 

        #region Theme

        Theme universityTheme = new Theme()
        {
            ThemeName = "University Life",
            ShortInformation = "Projects related to various aspects of university life",
            IsHeadTheme = true,

            Subthemes = new List<Theme>()
            {
                new Theme
                {
                    ThemeName = "Campus Facilities",
                    ShortInformation = "Projects related to the facilities available on campus"
                },
                new Theme
                {
                    ThemeName = "Academic Programs",
                    ShortInformation = "Projects related to the different academic programs offered"
                },
                new Theme
                {
                    ThemeName = "Student Services",
                    ShortInformation = "Projects related to the services provided to students"
                },
                new Theme
                {
                    ThemeName = "Research Opportunities",
                    ShortInformation = "Projects related to research opportunities at the university"
                },
                new Theme
                {
                    ThemeName = "Student Clubs and Organizations",
                    ShortInformation = "Projects related to student-run clubs and organizations"
                },
            }
        };

        foreach (var subtheme in universityTheme.Subthemes)
        {
            phygitalDbContext.Themes.Add(subtheme);
        }

        phygitalDbContext.Themes.Add(universityTheme);
        phygitalDbContext.SaveChanges();

        #endregion

        #region Question_Conditional

        QuestionWithOption universityFacilitiesQuestion = new QuestionWithOption
        {
            QuestionText = "Which of the following university facilities do you use regularly?",
            QuestionType = QuestionType.Multiple,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption { TextAnswer = "Library" },
                new AnswerOption { TextAnswer = "Gym" },
                new AnswerOption { TextAnswer = "Cafeteria" },
                new AnswerOption { TextAnswer = "Study Rooms" }
            }
        };

        phygitalDbContext.QuestionWithOptions.Add(universityFacilitiesQuestion);

        QuestionWithOption studentServicesSatisfactionQuestion = new QuestionWithOption
        {
            QuestionText = "Are you satisfied with the university's student services?",
            QuestionType = QuestionType.Single,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption { TextAnswer = "Yes" },
                new AnswerOption { TextAnswer = "No" }
            }
        };

        phygitalDbContext.QuestionWithOptions.Add(studentServicesSatisfactionQuestion);

        phygitalDbContext.SaveChanges();

        #endregion

        #region Step_Conditional

        Step universityFacilitiesStep = new Step
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "University Life"),
            Content = universityFacilitiesQuestion,
            StepState = State.Open,
            IsConditioneel = true,
            NextStep = null,
            Name = "University Facilities Usage"
        };

        phygitalDbContext.Steps.Add(universityFacilitiesStep);

        Step studentServicesSatisfactionStep = new Step
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "University Life"),
            Content = studentServicesSatisfactionQuestion,
            StepState = State.Open,
            IsConditioneel = true,
            NextStep = null,
            Name = "Student Services Satisfaction"
        };

        phygitalDbContext.Steps.Add(studentServicesSatisfactionStep);

        phygitalDbContext.SaveChanges();

        #endregion

        #region conditional_point

        ConditionalPoint universityFacilitiesPoint = new ConditionalPoint
        {
            ConditionalStep = universityFacilitiesStep,
            ConditionalPointName = "University Facilities Usage Point",
        };

        phygitalDbContext.ConditionalPoints.Add(universityFacilitiesPoint);

        ConditionalPoint studentServicesSatisfactionPoint = new ConditionalPoint
        {
            ConditionalStep = studentServicesSatisfactionStep,
            ConditionalPointName = "Student Services Satisfaction Point",
        };

        phygitalDbContext.ConditionalPoints.Add(studentServicesSatisfactionPoint);

        phygitalDbContext.SaveChanges();

        #endregion

        #region Question_single

        QuestionWithOption universityLifeQuestion1 = new QuestionWithOption
        {
            QuestionText = "What is your favorite aspect of university life?",
            QuestionType = QuestionType.Single,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption { TextAnswer = "Academic learning" },
                new AnswerOption { TextAnswer = "Social activities" },
                new AnswerOption { TextAnswer = "Research opportunities" },
                new AnswerOption { TextAnswer = "Campus environment", ConditionalPoint = universityFacilitiesPoint }
            }
        };

        phygitalDbContext.QuestionWithOptions.Add(universityLifeQuestion1);
        phygitalDbContext.SaveChanges();

        QuestionWithOption universityLifeQuestion3 = new QuestionWithOption
        {
            QuestionText = "Which university department do you think needs the most improvement?",
            QuestionType = QuestionType.Single,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption { TextAnswer = "Engineering" },
                new AnswerOption { TextAnswer = "Arts and Humanities" },
                new AnswerOption { TextAnswer = "Sciences" },
                new AnswerOption { TextAnswer = "Business" },
                new AnswerOption { TextAnswer = "Health and Medicine" }
            }
        };

        phygitalDbContext.QuestionWithOptions.Add(universityLifeQuestion3);
        phygitalDbContext.SaveChanges();

        #endregion

        #region Question_multi

        QuestionWithOption academicProgramsQuestion = new QuestionWithOption
        {
            QuestionText = "Which academic program do you think needs the most improvement?",
            QuestionType = QuestionType.Single,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption { TextAnswer = "Computer Science" },
                new AnswerOption { TextAnswer = "Business Administration" },
                new AnswerOption { TextAnswer = "Biology" },
                new AnswerOption { TextAnswer = "Psychology" },
                new AnswerOption { TextAnswer = "English Literature" }
            }
        };

        phygitalDbContext.QuestionWithOptions.Add(academicProgramsQuestion);

        QuestionWithOption studentClubsQuestion = new QuestionWithOption
        {
            QuestionText = "Which student clubs are you interested in joining?",
            QuestionType = QuestionType.Multiple,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption { TextAnswer = "Coding Club" },
                new AnswerOption { TextAnswer = "Debate Club" },
                new AnswerOption { TextAnswer = "Photography Club" },
                new AnswerOption { TextAnswer = "Dance Club" },
                new AnswerOption { TextAnswer = "Book Club" }
            }
        };

        phygitalDbContext.QuestionWithOptions.Add(studentClubsQuestion);

        phygitalDbContext.SaveChanges();

        #endregion

        #region Question_range

        QuestionWithOption universityLifeQuestion2 = new QuestionWithOption
        {
            QuestionText = "How satisfied are you with the university's facilities?",
            QuestionType = QuestionType.Range,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption { TextAnswer = "Very satisfied" },
                new AnswerOption { TextAnswer = "Somewhat satisfied" },
                new AnswerOption { TextAnswer = "Neutral" },
                new AnswerOption { TextAnswer = "Somewhat dissatisfied" },
                new AnswerOption { TextAnswer = "Very dissatisfied" }
            }
        };

        phygitalDbContext.QuestionWithOptions.Add(universityLifeQuestion2);

        phygitalDbContext.SaveChanges();

        QuestionWithOption academicStaffQuestion = new QuestionWithOption
        {
            QuestionText = "On a scale of 1-5, how would you rate your satisfaction with the academic staff?",
            QuestionType = QuestionType.Range,
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption { TextAnswer = "1" },
                new AnswerOption { TextAnswer = "2" },
                new AnswerOption { TextAnswer = "3" },
                new AnswerOption { TextAnswer = "4" },
                new AnswerOption { TextAnswer = "5" }
            }
        };

        phygitalDbContext.QuestionWithOptions.Add(academicStaffQuestion);

        phygitalDbContext.SaveChanges();

        #endregion

        #region Question_open

        QuestionOpen universityLifeQuestion4 = new QuestionOpen
        {
            QuestionText = "What changes would you like to see at the university?",
            QuestionType = QuestionType.Open
        };

        #endregion

        #region Information

        InformationContent universityInformation1 = new InformationContent
        {
            TextInformation =
                "Welcome to the University Life project! This project aims to gather feedback on various aspects of university life, including campus facilities, academic programs, student services, research opportunities, and student clubs and organizations. Your participation is valuable and will help improve the university experience for all students.",
        };

        phygitalDbContext.InformationContent.Add(universityInformation1);
        phygitalDbContext.SaveChanges();

        InformationContent themeStatisticsInformation = new InformationContent
        {
            TextInformation =
                "According to our data, 80% of students are satisfied with the university's facilities. However, only 60% of students are satisfied with the academic staff. We are working hard to improve these numbers."
        };

        phygitalDbContext.InformationContent.Add(themeStatisticsInformation);
        phygitalDbContext.SaveChanges();

        #endregion

        #region Steps

        Step universityLifeStep1 = new Step
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "University Life"),
            Content = universityLifeQuestion1,
            StepState = State.Open,
            IsConditioneel = false,
            NextStep = null,
            Name = "University Life Question 1"
        };

        Step universityLifeStep2 = new Step
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Campus Facilities"),
            Content = universityLifeQuestion2,
            StepState = State.Open,
            IsConditioneel = false,
            NextStep = null,
            Name = "University Life Question 2"
        };

        Step universityLifeStep3 = new Step
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Campus Facilities"),
            Content = universityLifeQuestion3,
            StepState = State.Open,
            IsConditioneel = false,
            NextStep = null,
            Name = "University Life Question 3"
        };

        Step universityLifeStep4 = new Step
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Academic Programs"),
            Content = academicProgramsQuestion,
            StepState = State.Open,
            IsConditioneel = false,
            NextStep = null,
            Name = "University Life Question 4"
        };

        Step universityLifeStep5 = new Step
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Student Clubs and Organizations"),
            Content = studentClubsQuestion,
            StepState = State.Open,
            IsConditioneel = false,
            NextStep = null,
            Name = "University Life Question 5"
        };

        Step universityLifeStep6 = new Step
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Academic Programs"),
            Content = academicStaffQuestion,
            StepState = State.Open,
            IsConditioneel = false,
            NextStep = null,
            Name = "University Life Question 6"
        };

        Step universityLifeStep7 = new Step
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "Student Services"),
            Content = universityLifeQuestion4,
            StepState = State.Open,
            IsConditioneel = false,
            NextStep = null,
            Name = "University Life Question 7"
        };

        Step informationStep = new Step
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "University Life"),
            Content = universityInformation1,
            StepState = State.Open,
            IsConditioneel = false,
            NextStep = universityLifeStep1,
            Name = "Information"
        };

        Step informationStep2 = new Step
        {
            Theme = phygitalDbContext.Themes.Single(th => th.ThemeName == "University Life"),
            Content = themeStatisticsInformation,
            StepState = State.Open,
            IsConditioneel = false,
            NextStep = universityLifeStep1,
            Name = "Information 2"
        };

        informationStep.NextStep = universityLifeStep1;
        universityLifeStep1.NextStep = universityLifeStep2;
        universityLifeStep2.NextStep = universityLifeStep3;
        universityLifeStep3.NextStep = universityLifeStep4;
        universityLifeStep4.NextStep = universityLifeStep5;
        universityLifeStep5.NextStep = informationStep2;
        informationStep2.NextStep = universityLifeStep6;
        universityLifeStep6.NextStep = universityLifeStep7;
        phygitalDbContext.Steps.AddRange(universityLifeStep1, universityLifeStep2, universityLifeStep3,
            universityLifeStep4, universityLifeStep5, universityLifeStep6, universityLifeStep7, informationStep,
            informationStep2);

        phygitalDbContext.SaveChanges();

        #endregion

        #region Flow

        Flow universityLifeFlow = new Flow
        {
            FlowName = "University Life",
            IsLinear = true,
            State = State.Open,
            FirstStep = informationStep,
            Steps = new List<Step>
            {
                informationStep,
                universityLifeStep1,
                universityLifeStep2,
                universityLifeStep3,
                universityLifeStep4,
                universityLifeStep5,
                universityLifeStep6,
                universityLifeStep7,
                universityFacilitiesStep,
                studentServicesSatisfactionStep
            }
        };

        Flow universityLifeFlow2 = new Flow
        {
            FlowName = "University Life 2",
            IsLinear = true,
            State = State.Open,
            FirstStep = null,
        };

        phygitalDbContext.Flows.Add(universityLifeFlow);
        phygitalDbContext.Flows.Add(universityLifeFlow2);
        phygitalDbContext.SaveChanges();

        #endregion

        #region Project

        Project universityLifeProject = new Project
        {
            Name = "University Life",
            IsActive = true,
            Theme = universityTheme,
            ProjectInformation = "This project aims to gather feedback on various aspects of university life",
            Flows = new List<Flow>
            {
                universityLifeFlow,
                universityLifeFlow2
            }
        };

        phygitalDbContext.Projects.Add(universityLifeProject);
        phygitalDbContext.SaveChanges();

        #endregion

        #region Users

        AttendentUser attendentUser3 = new AttendentUser()
        {
            FirstName = "John",
            LastName = "Walker",
            UserName = "johnwalker@universitylife.com",
            Email = "johnwalker@universitylife.com",
            EmailConfirmed = true,
            PhoneNumber = "+32466798529",
            BirthDate = new DateOnly(1999, 06, 24),
            TwoFactorEnabled = false,
            Organization = "University Life",
            AssignedProject = universityLifeProject
        };
        phygitalDbContext.AttendentUsers.Add(attendentUser3);
        userManager.CreateAsync(attendentUser3, "Password0!").Wait();
        userManager.AddToRoleAsync(attendentUser3, CustomIdentityConstants.Attendent).Wait();

        OrganizationUser organizationUser3 = new OrganizationUser()
        {
            FirstName = "Sarah",
            LastName = "Johnson",
            UserName = "Sarah.Johnson@universitylife.com",
            Email = "Sarah.Johnson@universitylife.com",
            EmailConfirmed = true,
            PhoneNumber = "+32499785299",
            BirthDate = new DateOnly(1987, 02, 16),
            TwoFactorEnabled = false,
            Organization = "University Life",
            OwnedProjects = new List<Project>()
            {
                universityLifeProject
            }
        };
        phygitalDbContext.OrganisationUsers.Add(organizationUser3);
        userManager.CreateAsync(organizationUser3, "Password00!").Wait();
        userManager.AddToRoleAsync(organizationUser3, CustomIdentityConstants.Organization).Wait();

        #endregion

        #region Platform

        Platform platform5 = new Platform()
        {
            IsHead = false,
            PlatformName = "University Life",
            CreationDate = new DateOnly(2024, 04, 23),
            OrganizationMaintainer = new List<OrganizationUser>()
            {
                organizationUser3
            },
            ProjectsAssigned = new List<Project>()
            {
                universityLifeProject
            }
        };

        phygitalDbContext.Platforms.Add(platform5);
        phygitalDbContext.SaveChanges();

        platformHead.SharingPlatforms.Add(platform5);

        HeadOfPlatformUser headOfPlatformUser2 = new HeadOfPlatformUser()
        {
            FirstName = "Jens",
            LastName = "Van Damme",
            UserName = "jensVanDamme@treecompany.be",
            Email = "jensVanDamme@treecompany.be",
            EmailConfirmed = true,
            PhoneNumber = "+32477458569",
            BirthDate = new DateOnly(1991, 12, 09),
            TwoFactorEnabled = false,
            ControlledPlatforms = new List<Platform>()
            {
                platform1,
                platform2,
                platform3,
                platform4,
                platform5,
                platformHead
            }
        };
        phygitalDbContext.HeadOfPlatformUsers.Add(headOfPlatformUser2);
        userManager.CreateAsync(headOfPlatformUser2, "Password8!").Wait();
        userManager.AddToRoleAsync(headOfPlatformUser2, CustomIdentityConstants.HeadOfPlatform).Wait();

        #endregion

        #region Comments

        Comment subComment2 = new Comment()
        {
            Text = "This is a comment to a comment",
            Likes = 5,
            User = applicationUser1
        };

        Comment headCommentUniversity = new Comment()
        {
            Text = "Comment about niversity life",
            Likes = 99,
        };

        phygitalDbContext.Comments.AddRange(subComment2, headCommentUniversity);
        phygitalDbContext.SaveChanges();
        Theme theme2 = phygitalDbContext.Themes.Include(t => t.Comments)
            .FirstOrDefault(th => th.ThemeName == "Academic Programs");
        theme2.Comments.Add(headCommentUniversity);
        phygitalDbContext.SaveChanges();

        #endregion
        
        #region Notes

        Note note1 = new Note
        {
            NoteTitle = "Ervaringen van Begeleiders bij het Stellen van de Vraag over het Droomstadspark aan Jongeren",
            NoteText =
                "Tijdens het afnemen van deze vraag bij jongeren, viel op dat er een grote verscheidenheid aan antwoorden werd gegeven. Veel jongeren gaven aan dat ze graag een skatepark zouden willen hebben, waar ze veilig kunnen skaten en BMX'en. Anderen noemden een groot sportveld voor voetbal en basketbal. Er was ook een opvallend aantal jongeren dat een plek wilde waar ze met vrienden kunnen chillen, zoals hangmatten of comfortabele zitzakken. Sommigen stelden voor om een plek voor graffiti en kunst te hebben, waar ze creatief bezig kunnen zijn. Al met al lieten de antwoorden zien dat jongeren veel waarde hechten aan actieve en sociale ruimtes in het park.",
            CreatedAttendantName = "Torriden.divein@gmail.com",
            Step = step16,
        };
        Note note2 = new Note
        {
            NoteTitle = "Jongerenvoorkeuren voor het Droomstadspark",
            NoteText =
                "Bij het stellen van deze vraag aan jongeren viel op dat er een sterke behoefte was aan voldoende groen en natuur. Veel jongeren gaven aan dat ze graag een rustig deel van het park zouden willen, met veel bomen, bloemen en vijvers waar ze kunnen ontspannen en tot rust kunnen komen. Er waren ook suggesties voor een gemeenschappelijke moestuin waar ze kunnen leren over tuinieren en gezonde voeding. De wens om meer natuur in de stedelijke omgeving te integreren was duidelijk een belangrijk thema voor de jongeren.",
            CreatedAttendantName = "Torriden.divein@gmail.com",
            Step = step16,
        };
        Note note3 = new Note
        {
            NoteTitle = "Verlanglijst van Jongeren voor het Droomstadspark",
            NoteText =
                "Tijdens de sessies met jongeren bleek dat veel van hen graag technologische innovaties in het park zouden willen zien. Er werden suggesties gedaan voor interactieve speeltoestellen, gratis Wi-Fi in het hele park, en oplaadpunten voor mobiele apparaten. Sommige jongeren wilden zelfs een outdoor gaming area met grote schermen en plekken om hun eigen apparaten aan te sluiten. Dit benadrukt de wens van de jongeren om hun digitale leven te kunnen integreren met hun buitenactiviteiten.",
            CreatedAttendantName = "Torriden.divein@gmail.com",
            Step = step16,
        };
        phygitalDbContext.Notes.AddRange(note1, note2, note3);

        #endregion   
        
        #region UniNotes

        Note unifNote1 = new Note
        {
            NoteTitle = "Ontevredenheid en Voorstellen",
            NoteText =
                "Een aantal studenten gaf aan enigszins tot zeer ontevreden te zijn met de faciliteiten. Veelgenoemde punten waren overvolle bibliotheken, verouderde laboratoria en een tekort aan laadpunten voor laptops en telefoons. Studenten stelden voor om meer moderne studieplekken te creÃ«ren, de infrastructuur voor duurzame energie te verbeteren en regelmatigere renovaties van de faciliteiten door te voeren om aan de hedendaagse normen te voldoen.",
            CreatedAttendantName = "Janine_Begeleider123@gmail.com",
            Step = universityLifeStep1
        };
        Note unifNote2 = new Note
        {
            NoteTitle = "Enkele Verbeterpunten",
            NoteText =
                "Sommige studenten waren enigszins tevreden maar zagen ruimte voor verbetering. Ze waardeerden de inspanningen van de universiteit, maar gaven aan dat er soms een gebrek is aan voldoende stille studieplekken, vooral tijdens de examenperiodes. Daarnaast werd er gewezen op de noodzaak voor meer diverse eetopties op de campus en betere onderhoud van de oudere gebouwen.",
            CreatedAttendantName = "Janine_Begeleider123@gmail.com",
            Step = universityLifeStep1
        };
        Note unifNote3 = new Note
        {
            NoteTitle = "Uitmuntende Faciliteiten",
            NoteText =
                "Veel studenten gaven aan dat ze zeer tevreden zijn met de faciliteiten van de universiteit. Ze prezen de moderne en goed onderhouden gebouwen, de ruime studieplekken en de uitstekende sportfaciliteiten. Ook de beschikbaarheid van technologisch geavanceerde apparatuur en snelle Wi-Fi werd als zeer positief ervaren. Studenten benadrukten dat deze voorzieningen hun studie-ervaring aanzienlijk verbeteren.",
            CreatedAttendantName = "Janine_Begeleider123@gmail.com",
            Step = universityLifeStep1
        };
        phygitalDbContext.Notes.AddRange(unifNote1, unifNote2, unifNote3);

        #endregion
    }
}