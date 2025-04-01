# CourseApp 2.0
## Learning to program C#

# Bug I have encountered and you may have and here is the solution

## Root IIS certificate issue

### Error encountered 

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
