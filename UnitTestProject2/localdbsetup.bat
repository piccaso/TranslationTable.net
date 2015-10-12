set
SqlLocalDB.exe c TTLocalDB
SqlLocalDB.exe h TTLocalDB TTLocalDB
SqlLocalDB.exe s TTLocalDB
SqlLocalDB.exe i TTLocalDB
SQLCMD.EXE -S "(localdb)\.\TTLocalDB" -Q "CREATE DATABASE TranslationTable;"
EXIT /B 0