export class User {
    id: number;
    access_token: string;
    refresh_token: string;
    expires_in: number; // in seconds

    static fromJSON(data: any) {
        return Object.assign(new this, data) as User;
    }
}