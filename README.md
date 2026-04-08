# 🧙‍♂️ Table Manager for GDR Events – Peter Pan Association

A desktop application developed in **C#** for the *Associazione Ludico Culturale Peter Pan* to simplify and automate the management of **GDR (tabletop role-playing games)** during live events and conventions.

## 🎯 Description

During GDR events, managing table bookings and participant orders was a manual and time-consuming task. This application solves the problem by integrating with **Eventbrite**, the platform where tickets and reservations are handled.

The app retrieves event and order data from Eventbrite, processes it, and stores it locally in a **SQLite** database. It provides an easy-to-use Windows interface to manage users, events, orders, and game tables.

## 🧩 Features

- **Eventbrite Integration**  
  Automatically syncs with Eventbrite to retrieve ticket reservations and event details.

- **Local Storage with SQLite**  
  All data is saved locally, allowing offline access and fast processing.

- **User-Friendly UI**  
  The application is divided into four main sections:
  - **Users**: Manage participants registered for the events.
  - **Events**: View and monitor upcoming and past GDR events.
  - **Orders**: Access and manage Eventbrite ticket orders.
  - **Tables**: Organize GDR tables, their schedules, and attendees.

- **Background Services**  
  Two background workers keep the data up to date:
  - Every **15 minutes**: Updates live event data.
  - Every **hour**: Performs a full synchronization of all events.

## 🖥️ Technologies

- **.NET (C#) WinUI 3**
- **SQLite** for local data persistence
- **Eventbrite API** for external data retrieval
- **Background Services** for automatic sync

## ❤️ Special Thanks

This project was created with love and dedication for the [**Associazione Ludico Culturale Peter Pan**](https://linktr.ee/peterpancdc), a passionate group of volunteers committed to spreading the joy of tabletop role-playing games.

May this tool help make your events smoother, your tables better organized, and your stories even more epic.


> **Note**: This application is designed for internal use by the Peter Pan association during GDR events. Make sure you have the proper credentials and access rights to Eventbrite.

## 📦 Data Flow Overview

```mermaid
flowchart TD
    A[🌐 Eventbrite Platform]
    B1[🔄 Background Service - Live Sync Every 15 min]
    B2[🕒 Background Service - Full Sync Every 1 hour]
    C[🗄️ Local SQLite Database]
    D1[👤 Users Page]
    D2[📅 Events Page]
    D3[📦 Orders Page]
    D4[🎲 Tables Page]

    A -->|Pulls data| B1
    A -->|Pulls data| B2

    B1 -->|Update live events| C
    B2 -->|Update all events| C

    C --> D1
    C --> D2
    C --> D3
    C --> D4
