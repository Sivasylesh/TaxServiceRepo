# TaxServiceRepo
ASP Dotnet-Core service to provide Tax calculation services


Steps to Run this Service:
1) Clone the Repo in local Machine
2) Open DBScript_TaxService.sql file which is present at this Repo. This Contains SQL Scripts to populate data in your Local Database.
![image](https://user-images.githubusercontent.com/119483908/205291265-d7a40ebd-bd1b-44d7-8ad2-3872a4ec3c0d.png)

3) Connect to your Local SQL Server and Execute DBScript_TaxService.sql file in SSMS. Now you should be able to see data like below
![image](https://user-images.githubusercontent.com/119483908/205291666-1f3d5926-8209-4010-95b1-7f2252fb9f17.png)


4) Open TaxService.sln in Visual Studio and Run the TaxService Project.
5) Modify the ConnectionString in appsettings.json to point to your Local SQL Server Connection.
![image](https://user-images.githubusercontent.com/119483908/205290799-24576a79-25a1-43f5-8030-4dc4a97536d3.png)


5) This Service contains Get Api for fetching tax details. Call the Api using browser or postman. 
Sample URL: https://localhost:44394/api/tax/getdetails?municipality=Vilnius&date=2020.01.01
![image](https://user-images.githubusercontent.com/119483908/205290590-beef3f71-8695-42cb-a2e5-78b16252bab4.png)


6) This Repo also contains Unit Test Cases.
![image](https://user-images.githubusercontent.com/119483908/205291096-f0d0a322-5b4f-48df-b5cf-03013a138421.png)

