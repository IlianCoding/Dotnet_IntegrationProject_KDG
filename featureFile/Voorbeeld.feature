Feature:    Als medewerker
  wil ik een overzicht hebben van alle boeken die uitgeleend zijn,
  zodat ik kan zien welke boeken nog niet terug zijn.

  Background:
    Given boeken
      | id | titel | auteur  | uitgeleend | uitleendatum |
      | 1  | boek1 | auteur1 | true       | 2023-12-01   |
      | 2  | boek2 | auteur2 | false      | 2023-11-01   |
      | 3  | boek3 | auteur3 | true       | 2023-12-01   |
      | 4  | boek4 | auteur4 | false      | 2023-11-01   |
      | 5  | boek5 | auteur5 | true       | 2023-12-01   |



  Scenario: Overzicht van uitgeleende boeken op datum van vandaag
    Given vandaag is "2023-12-06"
    When ik het overzicht van uitgeleende boeken opvraag
    Then krijg ik een lijst met 3 boeken