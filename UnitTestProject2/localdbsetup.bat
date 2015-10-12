SqlLocalDB.exe s MSSQLLocalDB
SqlLocalDB.exe i MSSQLLocalDB
SQLCMD.EXE -S "(localdb)\MSSQLLocalDB" -Q "CREATE DATABASE TranslationTable;"
EXIT /B 0