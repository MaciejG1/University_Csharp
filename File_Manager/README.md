# File Manager 

## Table of Contents
- [Introduction](#introduction)
- [Technologies](#technologies)
- [Features](#features)
- [Class Overview](#class-overview)
- [Project Status](#project-status)
- [Layout of Application](#layout-of-application)

## Introduction
**File Manager** is a dual-pane file management application, designed in the style of Total Commander, built with C# and WPF. The project aims to replicate core functionalities of file management tools, such as file copying, renaming, deletion, and folder creation, within a clean and accessible interface. This application is resilient against basic errors and supports convenient drag-and-drop functionality, enhancing the user experience.

## Technologies
This project was developed with:
- **Programming Language**: C# (.NET Framework)
- **Framework**: WPF (Windows Presentation Foundation)

## Features
The application provides essential file management features, including:
- **Dual-pane view**: Displays directories and files in two separate panels for easy navigation and operations between directories.
- **File and folder operations**:
  - **Copy**: Copies a selected file or folder from one panel to the currently open directory in the other panel.
  - **Rename**: Allows renaming of the selected file or folder via a pop-up window.
  - **Delete**: Deletes the selected file or folder.
  - **New Folder**: Creates a new folder in the currently open directory.
- **Context menu support**: Each operation can be performed by right-clicking on a file or folder.
- **Keyboard shortcuts**:
  - **F7**: Create a new folder.
  - **F8**: Delete a selected item.
  - **F5**: Refresh the list of the files and folders
- **Drag-and-drop functionality**: Easily copy files or folders between panels by dragging and dropping.

## Class Overview
Below is an overview of the main classes and their responsibilities:

- **FilesManager**: Contains the core logic for file management operations, including copying, renaming, deleting, and creating new folders.
- **MainWindow**: Initializes two instances of `FilesManager`, providing a dual-pane layout similar to Total Commander.
- **RenameWindow**: Defines the pop-up window for renaming files and folders, including UI and functionality for name entry and confirmation.
- **App.xaml / App.xaml.cs**: Manages application-wide resources, such as styles for UI components.

## Project Status
Current status: **Near completion**  
The application is functional but may receive additional features and improvements in future updates.

## Layout of Application
The layout includes two side-by-side panels displaying directories and files, with buttons below each panel for performing key file operations. The interface maintains a streamlined and user-friendly design, focusing on ease of use.

<img width="960" alt="image" src="https://github.com/user-attachments/assets/e826825b-69fd-44a2-9072-8ef96d1b0baf">

