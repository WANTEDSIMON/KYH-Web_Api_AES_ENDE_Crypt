﻿# Web_Api_AES_ENDE_Crypt

## Overview

This project is a minimal ASP.NET Core Web API that provides AES encryption and decryption services while securely managing encryption keys.
The system generates a random AES-256 key, stores it in an Excel file, and allows encryption and decryption using this key.

--- 
 <!-- Step 1 -->

### Initial Setup

1. First, make a directory to work in using:

```bash
mkdir dotnet_Api
```

2. Go to the folder we just created:

```bash
cd dotnet_Api
```

3. Create a folder to work directly in with the command:

```bash
mkdir Web_Api_AES_ENDE_Crypt
```

4. Move into the new folder using:

```bash
cd Web_Api_AES_ENDE_Crypt
```

5. Created the project using:

```bash
dotnet new web
```

6. Opened the project in Visual Studio:

```bash
start devenv .
```
> [!NOTE]
> Opened the project in VS Code:

```bash
code .
```
7. Generated a `.gitignore` file using:

```bash
dotnet new gitignore
```

   This was done to ensure that certain files and folders, such as `bin/` and `obj/` directories, are not added to version control. These folders contain build artifacts and temporary files that do not need to be tracked in Git.

8. Added a `README.md` file:

```bash
echo "# Web_Api_AES_ENDE_Crypt" >> README.md
```
9. Created a new repository on GitHub:

    - Go to GitHub and click on **New Repository**.
    - Name the repository `KYH_Web_Api_AES_ENDE_Crypt`.
    - Check the box to add a **README file**, as this will help with immediate repository documentation.
    - Do not add a **.gitignore** or **license** since they already exist locally.

--- 