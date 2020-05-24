conn = new Mongo();

db = conn.getDB("network");

db.posts.insertOne(
    {
        title: "Post 1",
        body: "Hello, this is test post 1.",
        category: "Post",
        createdDate: Date.now.toString(),
        owner: "sofa",
        comments:[{
            body: "Cool",
            ownerName: "Sofia",
            ownerSurname: "Lehka",
            createdDate: Date.now.toString()
        },
        {
            body: "Test comment",
            ownerName: "Test name",
            ownerSurname: "TestSurname",
            createdDate: Date.now.toString()
        }],
        likes:[
            {
                OwnerUsername: "sofa",
                Liked: Date.now.toString() 
            }
        ]
    }
);

db.posts.insertOne(
    {
        title: "Admin post",
        body: "Hello, this is admin post.",
        category: "Post",
        createdDate: Date.now.toString(),
        owner: "admin",
        comments:[{
            body: "Cool",
            ownerName: "Sofia",
            ownerSurname: "Lehka",
            createdDate: Date.now.toString()
        },
        {
            body: "Nice post",
            ownerName: "Taras",
            ownerSurname: "Sharko",
            createdDate: Date.now.toString()
        }],
        likes:[
            {
                OwnerUsername: "sofa",
                Liked: Date.now.toString() 
            }
        ]
    });

db.users.insertOne(
    {
     name: "Sofia",
     surname: "Lehka",
     username: "sofa",
     password: "1111",
     email: "legkasofia@gmail.com",
     age: 20,
     DateOfBirth: Date.UTC(1999,11,18).toString(),
     usersPosts: [],
     friends: []
    }
);
db.users.insertOne({
    name: "Admin",
    surname: "Admin",
    username: "admin",
    password: "admin",
    email: "admin@gmail.com",
    age: 20,
    DateOfBirth: Date.now().toString(),
    usersPosts: [],
    friends: []
});

db.users.insertOne(
    {
        name: "Taras",
        surname: "Sharko",
        username: "taras",
        password: "taras",
        email: "sharko197807@gmail.com",
        age: 19,
        DateOfBirth: Date.UTC(2000,10,7).toString(),
        usersPosts: [],
        friends: []

});

function addFriend(userName, friendUserName){
    cursor = db.users.find({username: friendUserName});
    if(cursor.hasNext()){
        var item = cursor.next();
        var data = {
            Id: item["_id"],
            friendName: item["name"],
            friendSurname: item["surname"],
            friendUsername: item["username"]
        };
        db.users.updateOne({
        _id: item["_id"]
        }, 
        {
            $push: {
            friends: {data}
        }});
    }    
}

addFriend("admin", "sofa");
addFriend("admin", "taras");

addFriend("taras", "sofa");

addFriend("sofa", "taras");
addFriend("sofa", "admin");