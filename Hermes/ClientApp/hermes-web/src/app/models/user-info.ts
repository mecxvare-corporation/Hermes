import { UserRelationship } from "./user-relationship";

export class UserInfoModel{
    constructor(
        public readonly id: string,
        public readonly firstName: string,
        public readonly lastName: string,
        public readonly profileImage: string,
        public readonly interests: string[],
        public readonly friends: UserRelationship[],
        public readonly followers: UserRelationship[],
    ){}
}