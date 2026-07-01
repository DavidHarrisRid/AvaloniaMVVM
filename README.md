# Avalonia MVVM Proof of Concept

This repository contains a small Avalonia-based desktop application that was created as a technical Proof of Concept for the Advanced Specialised Project.

The purpose of this project is not to build a complete production application. Instead, it prepares and verifies a reusable MVVM application structure that can later be used as a foundation for a modular Data Aggregator desktop application.

## Project Goal

The main goal of this Proof of Concept is to explore and validate the basic structure of an Avalonia desktop application using the MVVM pattern.

The project focuses on:

* Avalonia UI application structure
* MVVM separation between Views and ViewModels
* Dependency Injection setup
* ViewModel-based navigation
* DataTemplates for View/ViewModel mapping
* Centralized resources, strings, and styles
* Basic UI state handling
* A simple in-memory model for API sources and requests
* Preparation for later integration into a larger Data Aggregator system

The application does not aim to implement the full backend connection or complete data aggregation workflow. Backend communication, external API access, database storage, and JSON processing are handled as separate Proofs of Concept within the wider project.

## Technical Context

This project is part of a larger learning and preparation phase for a modular Data Aggregator.

The planned final system consists of several technical areas:

* External API data sources
* Backend processing
* Database storage
* JSON-based data handling
* Avalonia desktop frontend
* MVVM-based user interface structure

This repository focuses specifically on the frontend architecture and MVVM preparation.

## Technologies Used

* C#
* .NET 9
* Avalonia UI
* CommunityToolkit.Mvvm
* Microsoft.Extensions.DependencyInjection
* Material.Icons.Avalonia

## Implemented Concepts

### MVVM Structure

The application separates UI logic into Views and ViewModels. Views define the visual layout, while ViewModels provide the data and commands required by the interface.

### Dependency Injection

Services and ViewModels are registered centrally during application startup. The main window receives its DataContext through the configured service provider.

### DataTemplates

Avalonia DataTemplates are used to map ViewModels to their corresponding Views. This allows the application to switch visible content based on the currently selected ViewModel instead of manually creating Views in code-behind.

### Main Navigation

A `MainNavigationService` controls the main application navigation. It switches between the main application areas, such as Dashboard, Settings, and About.

The main ViewModel listens to navigation changes and updates the selected state of the sidebar buttons.

### Source and Request Area

The Dashboard contains a simple prototype structure for managing API-related objects.

It includes:

* API source models
* API request models
* simple source creation
* simple request creation
* source/request selection
* edit modes
* a secondary navigation area for request-related content

This is implemented as an in-memory structure and is intended to demonstrate UI flow and data binding rather than persistent storage.

### Resources and Styling

The project uses centralized resource dictionaries for colors, styles, and UI strings. This keeps visual styling and visible text separate from the main Views and prepares the application for future extension.

## Current Scope

This Proof of Concept demonstrates that a basic Avalonia MVVM application structure can be built and extended in a modular way.

The current scope includes:

* application startup
* dependency injection
* main window layout
* sidebar navigation
* dynamic content switching
* status bar area
* dashboard structure
* source/request models
* simple in-memory data handling
* centralized styles and resources

## Out of Scope

The following features are intentionally not implemented in this Proof of Concept:

* full backend API communication
* database persistence
* authentication
* production-ready error handling
* complete JSON data visualization
* complete Data Aggregator functionality

These topics are planned or explored in separate Proofs of Concept within the wider Advanced Specialised Project.

## Relevance for the Major Project

This repository provides a technical foundation for the later Major Project. It verifies that Avalonia, MVVM, Dependency Injection, DataTemplates, and ViewModel-based navigation can be used to structure the frontend of a modular Data Aggregator.

The results of this Proof of Concept can be reused and expanded in the later project phase, where the frontend will be connected to backend services, external data sources, and persisted JSON-based data.

## Status

Prototype / Proof of Concept

This project is intended for learning, architectural preparation, and technical validation.
