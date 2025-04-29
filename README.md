## CourseApp 2.0
### Learning to program C# with this [tutorial](https://www.youtube.com/watch?v=BfEjDD8mWYg&ab_channel=freeCodeCamp.org)

# Bug I have encountered and you may have and here is the solution

## Root IIS certificate issue

### Error encountered & Images
![Image1](https://github.com/tltommu/CourseApp2.0/blob/master/CourseApp2.0/Screenshots/image1.png?)  
Clicked yes for this error  

![Image2](https://github.com/tltommu/CourseApp2.0/blob/master/CourseApp2.0/Screenshots/image2.png?)  
Clicked yes as well

![Image3](https://github.com/tltommu/CourseApp2.0/blob/master/CourseApp2.0/Screenshots/image3.png?)  
Clicked yes 

![Image4](https://github.com/tltommu/CourseApp2.0/blob/master/CourseApp2.0/Screenshots/image4.png?)  
Clicked OK and you need to resolve it

![Image5](https://github.com/tltommu/CourseApp2.0/blob/master/CourseApp2.0/Screenshots/image5.png?)  
Error displayed on localhost with detailed message, this is where you check the certificate details and import them into the trust root certificate following the steps below.

Resolving steps:
1. Pressing `⊞ + r`, type `mmc` and press `enter`
2. In the MMC window, go to File > Add/Remove Snap-in...
3. Select `Certificates` from the list of available snap-ins, and click `Add`.
4. Select the Account that you use, for my case it was `my-user account` and click Next.
5. Select `Local Computer` and click `Finish`, then click OK to close the `Add/Remove Snap-in window`.
6. Navigate to `Certificates (Local Computer)` > `Trusted Root Certification Authorities` > `Certificates`.
7. Right-click in the right pane, select `All Tasks`, and click `Import`.
8. Export the self-signed certificate in advance (read the description of the certificate that is missing), and then follow the wizard to import the self-signed certificate.

Issue resolved based on procedures on this link[https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/installation/warnings-untrusted-certificate](https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/installation/warnings-untrusted-certificate)

## Scafoldded Items

All the pages in `Areas\Identity\Pages\Account\Manage` are scafoldded items of ASP.NET SDK. It normally would not appear in the files unless you added them manually by 
1. Right-clicking your project in solution explorer (in my case is `CourseApp2.0`)
2. Click `Add Scafoldded Item`
3. Select `Identity`
4. Then choose pages you want to add. There are almost 30 pages of it and you can customize them as you wish.

## Authentication
Google auth2.0 setup, follow this tutorial by [codewolf](https://www.youtube.com/watch?v=O1QmK_q2Xfw&ab_channel=TheCodeWolf) as well as the [offcial documentation](https://learn.microsoft.com/en-us/azure/app-service/overview-authentication-authorization#identity-providers) to setup.

### Dependency: Based on your ASP.NET version, in my case it was .NET6.0, you have to install the dependency `Microsoft.AspNetCore.Authentication.Google v6.0.x`to use the `AddGoogle` function.

## Attention
Please git ignore your `appsettings.json` and `appsettings.json.development`, don't commit it to public github repo like I did.  
Inside there is a string that is created for the test database if you followed the tutorial linked in this repo. You will need to modify it accordingly when you are deploying your own database.  
I recommend that you should download [SQL server management studio](https://learn.microsoft.com/en-us/ssms/download-sql-server-management-studio-ssms) & [SQL server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) for local database testing and future database management when you are deploying the database server.

## Sidenote 1 (as of 3 April,2025)
Added googleoAuth2.0 for authentication, however user still need to register to use the CRUD function of the app. Need fixes in the future.

## Sidenote 2 ( As of 4 April, 2025)
Finally Successfully connected to a on premise SQL database, thank you [DigitalTechJoint](https://www.youtube.com/watch?app=desktop&v=jT8eA9A7qXE&ab_channel=DigitalTechJoint)'s guide to help me pinpointing bug was wrong connectionstring.

## Sidenote 3 ( As of 20 April, 2025)
Successfully using google authentication to verify and create account for users to leave comment. Added basic app logic such that only the creator of the comment can edit/delete their comment.
