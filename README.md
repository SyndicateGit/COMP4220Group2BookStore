# Setup:
1. Clone repository to local folder.
```
git clone https://github.com/SyndicateGit/COMP4220Group2BookStore.git
```
2. Open in Visual Studio
   Double click rzBookstore.sln solutions file to open.
4. Set up DB
  Under BookStoreLib > Properties > Settings.settings:
    - Open Settings.settings
    - Go to value column of RZConnection to open up connection properties.
      ![image](https://github.com/user-attachments/assets/23bcc1a7-c552-4e53-bef4-f7223635c292 )
    - Change the location to the xyBookstoreDB.mdf file in your project folder
    - Test Connection (should work) and click OK.
5.  Click green right arrow start button at the top to launch program.

Follow announcement for tfs.Agile2DB24 database setup.
Add to BookStoreLib > Properties > Settings.setting a new connection string to tfs.cs.uwindsor.ca under MSSQLConnection.
![image](https://github.com/user-attachments/assets/87627bab-8fe5-4c4c-9f2f-4f0be202352b)
![image](https://github.com/user-attachments/assets/f7d76dc0-2c9a-46b2-a3f4-1c3117aa5873)




