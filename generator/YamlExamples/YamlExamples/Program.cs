﻿// See https://aka.ms/new-console-template for more information
using yamlGen;

Console.WriteLine("Hello, World!");
bool dryRun = true;
DoIt doingIt = new DoIt(@"C:\github.com\statisticssweden\api2Spec\yamlGen\PxApiSpecs\",dryRun);
