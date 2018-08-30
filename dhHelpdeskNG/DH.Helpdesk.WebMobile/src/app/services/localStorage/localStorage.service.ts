import { CurrentUser } from '../../models'
import { AuthConstants } from '../../helpers/constants'

export class LocalStorageService {
    constructor() {}

    getCurrentUser(): CurrentUser {
        let currentUser = localStorage.getItem(AuthConstants.userDataStorageName);
        if(currentUser) {
            return CurrentUser.fromJSON(currentUser);
        }

        return null;
    }

    setCurrentUser(user: CurrentUser) {
        localStorage.setItem(AuthConstants.userDataStorageName, JSON.stringify(user));
    }

    removeCurrentUser() {
        localStorage.removeItem(AuthConstants.userDataStorageName);
    }
}