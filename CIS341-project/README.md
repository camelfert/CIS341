This is a basic blog web application based on ASP.NET Core 7.0 and Microsoft Identity, built as a final project for CIS341 - Interactive Web Programming at UW-Stevens Point.

The app features CRUD functionality for all aspects, including Blog Posts, Comments (with replies), Post/Comment reactions ala Reddit upvotes/downvotes, and basic admin capabilities to ban/unban users.

The app database is seeded upon first start with an admin account that allows all app functionality as well as sample posts, comments, and reactions to both. 

The login credentials are as follows:

Username/Email: admin@lunchbox.com
Password: AdminPass123!

To leave a comment, or react to either, a user must be registered and logged in.
Users with the Admin role can create, edit, and delete blog posts as well as edit/delete other users' comments and reset post/comment reactions.
Admin role users also have access to the Admin Panel, which lists all users within the database and lets roles be added/removed on a per-user basis.