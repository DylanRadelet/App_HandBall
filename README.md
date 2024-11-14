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

## Testing 

Site test : https://www.apphandball.nerwii.com/

## Probleme dans l'application 

1. Au moment ou j'affiche :
Console.WriteLine($"Équipe 1 : {teamHeim}");
Console.WriteLine($"Équipe 2 : {teamGast}");
Console.Write("Sélectionnez votre équipe (1 ou 2) : ");
Apres 10-15 secondes j'ai des messages qui s'affiche. Si quelqu'un à une idée pour regler ça je suis prenant ! 
Les messages en question :
1. Created TensorFlow Lite XNNPACK delegate for CPU.
2. Attempting to use a delegate that only supports static-sized tensors with a graph that has dynamic-sized tensors (tensor#58 is a dynamic-sized tensor).
