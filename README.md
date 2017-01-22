# VbProjectParser
C# library to read Vba code / VBA Project information from VBProject.bin or regular Excel files. It has no requirements on MS Excel being installed on machine.

This project demonstrates how to read basic information from VBProject.bin binary files, e.g. to extract VBA Code & module information as plain text.

It contains an integration project with OpenXML, so the same can be done for regular Excel files without manually having to extract the VBProject files. As a result, it is possible to e.g. read VBA Code from Excel files without opening these in Excel (or even having Excel installed on the machine).

All code is full C#. This project is work in progress, please report issues on the Github issue tracker.
