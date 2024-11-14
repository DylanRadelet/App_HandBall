# Match Event PDF Generator

Cette application console génère un fichier PDF qui résume les événements d'un match à partir d'une URL fournie. Elle utilise le navigateur Chrome pour extraire les données de l'URL et enregistre les événements dans un tableau PDF. L'application permet également d'ajouter manuellement des événements pour compléter le rapport.

## Fonctionnalités

- **Extraction des événements depuis une URL** : utilise Selenium pour récupérer la source HTML d'une page de match et HtmlAgilityPack pour parser les informations.
- **Génération de PDF** : utilise iTextSharp pour créer un PDF avec un tableau des scores et des événements.
- **Entrée manuelle d'événements** : ajoutez des événements manuellement si certains ne sont pas extraits automatiquement.

## Prérequis

Assurez-vous d'avoir installé les packages suivants :

- **iTextSharp** pour la génération de PDF. ( ```dotnet add package iTextSharp``` )
- **HtmlAgilityPack** pour le parsing HTML. ( ```dotnet add package HtmlAgilityPack``` )
- **Selenium WebDriver** pour la navigation automatique (et le driver Chrome). ( ```dotnet add package Selenium.WebDriver``` )
- **Google Chrome** (et le chemin du driver Chrome dans votre PATH).

## Installation

1. Cloner le repository.

##Testing 

Site test : https://www.apphandball.nerwii.com/
