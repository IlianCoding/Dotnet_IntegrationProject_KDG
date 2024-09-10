Feature: Afronding en bedanking aan het einde van de flow

  Background:
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
      | project_id | project_naam | isActive | deelplatform_id |
      | 201        | ProjectA     | true     | 301             |
      | 202        | ProjectB     | false    | 302             |
      | 203        | ProjectC     | true     | 303             |

    Given themas
      | thema_id | thema_naam | isHoofdThema | project_id |
      | 401      | Thema1     | true         | 201        |
      | 402      | Thema2     | false        | 202        |
      | 403      | Thema3     | true         | 203        |

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

    Given questions
      | question_id | question_description                                                | type_question     | flow_id |
      | 1           | "Wat is het beste transport van jouw stad"                          | "Single_choice"   | 101     |
      | 2           | "Wat vind je van het transport in jouw stad"                        | "Open"            | 101     |
      | 3           | "Wat zou je willen zien in het park dat in 2025 gebouwd zal worden" | "Multiple_choice" | 103     |

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
    Given steps
      | step_id | stepDescription |
      | 1       | vraag1          |
      | 2       | antwoord1       |
      | 3       | Afronding       |


  Scenario: Duidelijke afronding en bedanking wordt getoond aan gebruiker
    Given gebruiker is aan step met id 2
    When de gebruiker zijn antwoord inlevert
    Then wordt de duidelijke afronding en bedanking getoond

  Scenario: Duidelijke afronding en bedanking wordt getoond aan gebruiker
    Given gebruiker aan sessie met id 501 is bezig met de laatste step van de flow, met id 2
    When de gebruiker zijn antwoord inlevert
    Then wordt de duidelijke afronding en bedanking getoond


  Scenario: Duidelijke afronding en bedanking wordt niet getoond aan gebruiker
    Given gebruiker aan sessie met id 504 is bezig met de laatste vraag  met id 1
    When de gebruiker zijn antwoord inlevert
    Then wordt de duidelijke afronding en bedanking niet getoond