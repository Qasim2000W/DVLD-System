ملف الباك أب الموجود في المستودع: DVLDNew.bak. هذا ملف احتياطي لقاعدة بيانات الاختبار/المشروع التدريبي فقط.

بيانات الاتصال (username / password) الموجودة في ملف إعدادات التطبيق (app config) هي قيم نائبة (placeholders). يجب استبدالها بمعلومات الاتصال الواقعية لقاعدة البيانات عند تشغيل التطبيق على جهازك أو على السيرفر. مثال قصير لصيغة اتصال عامة (SQL Server):

Server=YOUR_DB_HOST;Database=DVLDNew;User Id=YOUR_DB_USER;Password=YOUR_DB_PASSWORD;
تجاوز شاشة تسجيل الدخول بهذه البيانات: Usernam=Qas  password=1234
ن أردت إنشاء مستخدم اختبار سريعاً:

INSERT INTO dbo.[users] (username, password, /* أعمدة أخرى حسب الجدول */)
VALUES ('testuser','testpassword' /*، قيم أعمدة أخرى */);
.
.
.
The backup file included in this repository is DVLDNew.bak. It is an archive of the project/test database for training/demo purposes only.

The username/password values in the app config are placeholders. Replace them with your actual database connection information when running the application. Generic SQL Server connection example:

Server=YOUR_DB_HOST;Database=DVLDNew;User Id=YOUR_DB_USER;Password=YOUR_DB_PASSWORD;
Bypass the login screen with this data: Username=Qas Password=1234
To create a quick test user:

INSERT INTO dbo.[users] (username, password, /* other columns as needed */)
VALUES ('testuser','testpassword' /*, other column values */);
