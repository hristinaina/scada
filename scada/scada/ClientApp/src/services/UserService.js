class UserService {
    constructor() {
        this.isAdmin = false;
    }

    setRole(user) {
        this.isAdmin = user.email === 'pera@gmail.com' ? true : false;
    }

    getRole() {
        return this.isAdmin;
    }
}

const userService = new UserService();
export default userService;
