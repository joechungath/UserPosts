# UserPublish
Refrence: https://itnext.io/how-to-build-an-event-driven-asp-net-core-microservice-architecture-e0ef2976f33f
RabbitMQ: http://localhost:15672/

The application uses a real-world example with users that can write posts. The user microservice allows creating and editing users. In the user domain, the user entity has several properties like name, mail, etc. In the post domain, there is also a user so the post microservice can load posts and display the writers without accessing the user microservice.
