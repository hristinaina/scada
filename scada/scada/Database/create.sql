create database scada;

use scada;

CREATE TABLE AlarmHistory (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Timestamp DATETIME NOT NULL,
    TagId INT NOT NULL,
    AlarmId INT NOT NULL
);

CREATE TABLE TagHistory (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Value DOUBLE NOT NULL,
    Timestamp DATETIME NOT NULL,
    TagId INT NOT NULL
);

CREATE TABLE Users (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Email VARCHAR(255) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Role ENUM('Admin', 'Client') NOT NULL
);
