set
SqlLocalDB.exe c TTLocalDB
SqlLocalDB.exe h TTLocalDB TTLocalDBShared
SqlLocalDB.exe s TTLocalDB
SqlLocalDB.exe i TTLocalDB
SQLCMD.EXE -S "(localdb)\.\TTLocalDBShared" -Q "CREATE DATABASE TranslationTable;"
EXIT /B 0