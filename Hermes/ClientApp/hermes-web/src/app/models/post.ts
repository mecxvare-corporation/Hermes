export class PostModel{
    constructor(
        public readonly id: string,
        public readonly title: string,
        public readonly content: string,
        public readonly userId: string,
        public readonly image: string | undefined,
        public readonly createdAt: Date,
        public readonly updatedAt: Date | undefined
    ){}
}