export class AuthentificationError extends Error {
    constructor(message: string) {
        super(message); 
        this.name = "AuthentificationError"; 
        Object.setPrototypeOf(this, AuthentificationError.prototype);
    }
}

export class InvalidFormatError extends Error {
    constructor(message: string) {
        super(message); 
        this.name = "InvalidFormat"; 
        Object.setPrototypeOf(this, InvalidFormatError.prototype);
    }
}

export class NotFoundError extends Error {
    constructor(message: string) {
        super(message); 
        this.name = "InvalidFormat"; 
        Object.setPrototypeOf(this, NotFoundError.prototype);
    }
}