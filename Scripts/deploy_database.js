conn = new Mongo();

db = conn.getDB("network");
db.users.drop();
db.posts.drop();

function add_user(username,password,email,name,surname,age,dateOfBirth,userPosts,userStatus,friends) {
    var obj = {
        username: username,
        password: password,
        email: email,
        name: name,
        surname: surname,
        age: age,
        dateOfBirth: dateOfBirth,
        userPosts: userPosts,
        userStatus: userStatus,
        friends: friends
    }

    db.users.insertOne(obj);
}

function addFriends(userName, friends)
{
    function addFriend(user, friend){
        var item = db.users.find({username: friend}).next();
        var data = {
            friendId: item["_id"],
            friendName: item["name"],
            friendSurname: item["surname"],
            friendUsername: item["username"]
        }
        db.users.updateOne({_id: user["_id"]},
        {
            $push: {
                friends: data
            }
        });
    }

    var user = db.users.find({username: userName}).next();
    
    friends.forEach(element => {
        addFriend(user,element);
    });
}

add_user("admin", "admin", "admin@sparkle.com", "Taras","Sharko","20", ISODate("1990-10-10"), [], 1, []);
add_user("sofa", "sofa", "legkasofia@gmail.com", "Sofia", "Lehka", "20", ISODate("1999-11-18"), [], 1, []);
add_user("pie", "pie", "pie@sparkle.com", "Pie", "Tree", "2", ISODate("2018-01-01"), [], 1, []);
add_user("ysharko","ysharko", "ysharko@sparkle.com", "Yulia", "Sharko", "15", ISODate("2005-03-09"), [],1,[]);
add_user("olena", "olena","olena@sparkle.com", "Olena", "Flower", "21", ISODate("1998-04-12"),[],1,[]);
add_user("stepan", "stepan","stepan@sparkle.com", "Stepan", "Step", "25", ISODate("1995-03-21"),[],1,[]);

addFriends("admin", ["sofa", "pie"]);
addFriends("sofa", ["admin"]);
addFriends("pie", ["ysharko","sofa"]);
addFriends("olena", ["ysharko"]);
addFriends("ysharko", ["olena"]);

