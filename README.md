## CourseApp 2.0
### Learning to program C#

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
