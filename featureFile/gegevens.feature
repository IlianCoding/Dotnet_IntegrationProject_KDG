Feature:
  Background:
    Given organisaties
      | userId | userName |
      | 301    | orgUser1 |
      | 302    | orgUser2 |
      | 303    | orgUser3 |

    Given projectbeheerders
      | userId | userNaam   |
      | 101    | beheerder1 |
      | 102    | beheerder2 |
      | 103    | beheerder3 |

    Given gebruiker
      | gebruiker_id | gebruiker_naam | email        | telefoonnummer |
      | 1            | Yoeri          | yoeri@kdg.be | +32499765876   |
      | 2            | Ruben          | ruben@kdg.be | +32566798541   |
      | 3            | Hanne          | hanne@kdg.be | +31477855645   |

    Given begeleiders
      | begeleider_id | begeleider_naam |
      | 1             | Begeleider1     |
      | 2             | Begeleider2     |
      | 3             | Begeleider3     |

    Given flows
      | flow_id | flow_naam | project_id | soort      |
      | 101     | Flow1     | 201        | lineare    |
      | 102     | Flow2     | 202        | circulaire |
      | 103     | Flow3     | 203        | circulaire |

    Given sessies
      | sessie_id | flow_id | isGepauzeerd |
      | 501       | 101     | false        |
      | 502       | 102     | true         |
      | 503       | 103     | false        |



    Given projecten
      | project_id | project_naam            | project_status | deelplatform_id |
      | 201        | ProjectA                | true           | 301             |
      | 202        | ProjectB                | false          | 302             |
      | 203        | ProjectC                | true           | 303             |
      | 1          | Verkiezingsanalyse      | true           | 301             |
      | 2          | Campagnestrategieën     | false          | 301             |
      | 3          | Stemprocesoptimalisatie | true           | 301             |

    Given themas
      | thema_id | thema_naam            | isHoofdThema | project_id |
      | 401      | Thema1                | true         | 201        |
      | 402      | Thema2                | false        | 202        |
      | 403      | Thema3                | true         | 203        |
      | 1        | Overheid Verkiezingen | true         | 1          |
      | 2        | Campagne Strategie    | false        | 2          |
      | 3        | Stemprocedure         | false        | 3          |
      | 101      | Kiescampagnes         | false        | 2          |
      | 102      | Verkiezingsresultaten | false        | 1          |
      | 103      | Stemproces            | false        | 3          |


    Given organisaties
      | organisatie_id | organisatie_naam |
      | 501            | Org1             |
      | 502            | Org2             |
      | 503            | Org3             |

    Given deelplatforms
      | deelplatform_id | deelplatform_naam | invoegen_na_stap |
      | 301             | PlatformA         | true             |
      | 302             | PlatformB         | false            |
      | 303             | PlatformC         | true             |

    Given vragen
      | vraag_id | vraag_beschrijving                                                  | vraag_type        | flow_id |
      | 1        | "Wat is het beste transport van jouw stad"                          | "Single_choice"   | 101     |
      | 2        | "Wat vind je van het transport in jouw stad"                        | "Open"            | 101     |
      | 3        | "Wat zou je willen zien in het park dat in 2025 gebouwd zal worden" | "Multiple_choice" | 103     |
      | 1        | Wat is het belang van verkiezingen?                                 | "Single_choice"   | 101     |
      | 2        | Wat zijn effectieve campagnestrategieën?                            | "Single_choice"   | 101     |
      | 3        | Hoe beïnvloedt het stemproces de verkiezingen?                      | "Single_choice"   | 101     |

    Given question_opties
      | option_id | option_description      | question_id |
      | 601       | "auto"                  | 1           |
      | 602       | "fiets"                 | 1           |
      | 603       | "bus"                   | 1           |
      | 604       | "tram"                  | 1           |
      | 605       | "trein"                 | 1           |
      | 606       | "speeltuin"             | 3           |
      | 607       | "Sportfaciliteiten"     | 3           |
      | 608       | "Picknickgebieden"      | 3           |
      | 609       | "Wandel- en fietspaden" | 3           |
      | 610       | "Botanische tuinen"     | 3           |

    Given informaties
      | informatieId | informatieTitle                |
      | 1            | Algemene Verkiezingsinformatie |
      | 2            | Campagnestrategieën            |
      | 3            | Stappen in het Stemproces      |
    Given teksten
      | informatieId | context                        | titel                                    |
      | 1            | Introductie                    | Over het belang van verkiezingen         |
      | 1            | Kiesstelsels wereldwijd        | Vergelijking van kiesstelsels            |
      | 2            | Doelgroepanalyse               | Identificeren van doelgroepen            |
      | 3            | Stemregistratie en Verificatie | Veiligheid en nauwkeurigheid van stemmen |

    Given afbeeldingen
      | informatieId | path                   | altcode                |
      | 1            | /images/intro.png      | ElectionIntroImage123  |
      | 2            | /images/doelgroep.png  | TargetAudienceImage456 |
      | 3            | /images/stemproces.png | VotingProcessImage789  |

    Given videos
      | informatieId | path                   | altcode                |
      | 1            | /videos/intro.mp4      | ElectionIntroVideo123  |
      | 2            | /videos/doelgroep.mp4  | TargetAudienceVideo456 |
      | 3            | /videos/stemproces.mp4 | VotingProcessVideo789  |

    Given audios
      | informatieId | path                   | altcode                |
      | 1            | /audios/intro.mp3      | ElectionIntroAudio123  |
      | 2            | /audios/doelgroep.mp3  | TargetAudienceAudio456 |
      | 3            | /audios/stemproces.mp3 | VotingProcessAudio789  |

