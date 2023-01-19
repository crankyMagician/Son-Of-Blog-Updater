# SonOfBlogUpdater
# BlogUpdater
 A CRUD Blog schema housed in Azure

# 🌊 Requirements: 🌊
 
 
- ⭐ Login – app requires users to login either via email or separate application service
   -  💫 User can reset password
-  ⭐ Roles- this application will require roles. 
   - 💥 User - user will be able to edit their own blog posts. They will be able to add, remove, and create tags to assign to their blogs posts. As well as edit their own profile picture
   - 💥 Admin - The admin will be able to perform actions on all user accounts as well as delete tags from the database. 
   

# 💾 Packages Needed: 💾
- 🔱Identity - a microsoft package to manage roles https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-7.0&tabs=visual-studio
- 📫Send Grid - a third party api to send mail. use the free plan, 100 emails a day -https://app.sendgrid.com/
- 👮 Authentication - use Microsoft's built in account protocol https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/?view=aspnetcore-7.0&tabs=visual-studio

# 🏘️ Architecture 🏘️
- 🕸️ ASP .NET - use .NET Core 7 and MVC paradigm to create a crud APP
- 🏚️ Azure - Azure has a lot of tools that work nicely with ASP .NET 
- 🏪MSSQL - A SQL Server hosted in AZURE 
- 🖥️ VM - The app will run on a server in the cloud : ) 
