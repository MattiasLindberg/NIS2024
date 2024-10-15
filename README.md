# NIS2024 - Key-less authorization between Azure App Services

Code and presentation for my session at Nordic Integration Summit 2024.

## Abstract
Using RBAC to assign access to resources is standard in Azure, but how 
do you do that between two Azure App Services? It is not uncommon to still 
see keys-based authentication to integrate an application with a backend API. 
In this session we will show how Entra ID App Registrations, custom roles, 
Managed Identity and a little code can help you eliminate passwords and 
access keys when you want two Azure App Services to communicate.
