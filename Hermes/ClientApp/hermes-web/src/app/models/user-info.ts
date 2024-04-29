import { PostModel } from "./post";
import { UserRelationship } from "./user-relationship";

export class UserModel{
    constructor(
        public readonly id: string,
        public readonly firstName: string,
        public readonly lastName: string,
        public readonly profileImage: string,
        // public readonly interests: string[],
        // public readonly friends: UserRelationship[],
        // public readonly followers: UserRelationship[],
        // public readonly posts: PostModel[],
    ){}
}