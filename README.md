# NIS2024 - Key-less authorization between Azure App Services

Code and presentation for my session at Nordic Integration Summit 2024.

Please find the presentation at [Key-less authorization between Azure App Services.pdf](https://github.com/MattiasLindberg/NIS2024/blob/main/Key-less%20authorization%20between%20Azure%20App%20Services.pdf)

## Abstract
Using RBAC to assign access to resources is standard in Azure, but how 
do you do that between two Azure App Services? It is not uncommon to still 
see keys-based authentication to integrate an application with a backend API. 
In this session we will show how Entra ID App Registrations, custom roles, 
Managed Identity and a little code can help you eliminate passwords and 
access keys when you want two Azure App Services to communicate.
