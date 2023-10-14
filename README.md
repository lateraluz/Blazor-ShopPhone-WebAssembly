# ShopPhone

ShopPhone Developed with Blazor for Mitocode Course

> [!NOTE]
> Just for educational purpose

# Video Demostration
Click on image below to see the video

[![IMAGE ALT TEXT](https://github.com/lateraluz/ShopPhone/blob/d0cbda8225af34b5af510c36b8ce1fdae7ca84b2/Images/dummyimage.jpg)](https://raw.githubusercontent.com/lateraluz/ShopPhone/master/Video/BlazorProject.mp4)




# Config
1. Create Folder at "C:\\Temp\\images" and copy all images (phone pictures)
2. Create site at http://localhost:8081/images in order to read the images
3. Restore Database ShopPhone.bak
4. Run the application

# Open Telemetry with Jagger (Docker)
1. docker run --rm --name jaeger  -e COLLECTOR_ZIPKIN_HOST_PORT=:9411  -p 6831:6831/udp  -p 6832:6832/udp   -p 5778:5778   -p 16686:16686   -p 4317:4317   -p 4318:4318   -p 14250:14250   -p 14268:14268 -p 14269:14269  -p 9411:9411   jaegertracing/all-in-one:1.50
2. http://localhost:16686
![1](https://github.com/lateraluz/Blazor-ShopPhone/blob/4c2ddd6a248c588b5c8d89ec483d237062a78ab6/Video/Telemery_Jaeger.png) 

References
https://www.jaegertracing.io/docs/1.50/getting-started/


# Application Users
admin
123456*

maria
123456*

# How to Check Healths
1. Health (raw json) <BR>
![1](https://github.com/lateraluz/Blazor-ShopPhone/blob/12afe94c8e7688c038e1c5bb496f4e2d5a122445/Video/Healthy.png)

2. Health UI <BR>
![2](https://github.com/lateraluz/Blazor-ShopPhone/blob/12afe94c8e7688c038e1c5bb496f4e2d5a122445/Video/Healthy-ui.png)


# To-Do
1. ~~Concurrency Control~~
2. Drag and drop images
3. Telemetry
4. Refresh API JWT
5. ~~Carrusel~~  Bootstrap 5.1 :(
6. ~~Test cases~~
7. ~~Integration Testing~~
8. Fix withdraw inventory
9. ~~Fluent Validators~~
10. Fix problem with catching errors thru layers.
11. ~~Change DTOs to records~~
11. ~~Tables Reponsive~~
12. ~~Pagination Reponsive~~
13. Create components in order to make reuse code.
14. Add Icons Button
15. Fix Algortim used in order to encrypt data "Warning": The default hash algorithm and iteration counts in Rfc2898DeriveBytes constructors are outdated and insecure. Use a constructor that accepts the hash algorithm and the number of iterations.
16. Error boundaries.
