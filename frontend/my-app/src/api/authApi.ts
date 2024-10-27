import axios, { AxiosResponse } from 'axios';

export interface LoginCredentials {
    email: string;
    password: string;
}

export interface RegisterCredentials {
    username: string,
    email: string;
    password: string;
}

export interface ResetPasswordCredentials {
    newPassword: string;
    confirmPassword: string;
    token: string;
}

export async function login(loginCredentials: LoginCredentials) {
    try {
        const response : AxiosResponse = await axios.post('https://localhost:5295/api/User/login', loginCredentials);
        if (response.status === 200) {
            console.log(response.data);
            return response.data;
        } else {
            throw new Error('Login failed.'); 
        }
        
    } catch (error: any) {
        if (axios.isAxiosError(error) && error.response) {
            console.log(error)
            const errorMessage = error.response.data.errors?.[0]?.description || 'An unexpected error occurred.';
            console.error('Error during login:', errorMessage);
            throw new Error(errorMessage);
        } else {
            console.error('Error during login:',);
            throw new Error('An unexpected error occurred.');
        }
    }
}

export async function register(registerCredentials: RegisterCredentials) {
    try {
        const response : AxiosResponse = await axios.post('https://localhost:5295/api/user/register', registerCredentials);
        if (response.status === 200) {
            console.log(response.data);
            return response.data;
        } else {
            throw new Error('Register failed.'); 
        }
        
    } catch (error: any) {
        if (axios.isAxiosError(error) && error.response) {
            console.log(error)
            const errorMessage = error.response.data.errors?.[0]?.description || 'An unexpected error occurred.';
            console.error('Error during register:', errorMessage);
            throw new Error(errorMessage);
        } else {
            console.error('Error during register:',);
            throw new Error('An unexpected error occurred.');
        }
    }
}

export async function forgotPassword(email : string) {
    try {
        const response : AxiosResponse = await axios.post("https://localhost:5295/api/user/forgot-password", { email : email}); 
        if (response.status === 200) {
            console.log(response.data);
            return response.data;
        } else {
            throw new Error('Forgot password request failed.'); 
        }
    } catch (error) {
        if (axios.isAxiosError(error) && error.response) {
            console.log(error)
            const errorMessage = error.response.data.errors?.[0]?.description || 'An unexpected error occurred.';
            console.error('Error during asking for a token to reset password:', errorMessage);
            throw new Error(errorMessage);
        } else {
            console.error('Error during asking for a token toreset password:',);
            throw new Error('An unexpected error occurred.');
        }
    }
}

export async function resetPassword(resetPasswordCredentials : ResetPasswordCredentials) {
    try {
        console.log('Resetting password')
        if(resetPasswordCredentials.newPassword !== resetPasswordCredentials.confirmPassword) {
            throw new Error("Password are not the same");
        }

        const response : AxiosResponse = await axios.post("https://localhost:5295/api/user/resetPassword", resetPasswordCredentials); 
        if (response.status === 200) {
            console.log(response.data);
            return response.data;
        } else {
            throw new Error('Reset Password request failed.'); 
        }

    } catch (error) {
        if (axios.isAxiosError(error) && error.response) {
            console.log(error)
            const errorMessage = error.response.data.errors?.[0]?.description || 'An unexpected error occurred.';
            console.error('Error during asking to reset password:', errorMessage);
            throw new Error(errorMessage);
        } else {
            console.error('Error during asking to reset password:',);
            throw new Error('An unexpected error occurred.');
        }
    }
}