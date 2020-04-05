@echo off
IF EXIST %1 GOTO Unzip
GOTO Koniec

:Unzip
%2

IF EXIST %3 GOTO Uruchom
GOTO Koniec

:Uruchom
%3

:Koniec