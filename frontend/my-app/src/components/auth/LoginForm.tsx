import '../../styles/form.css';
import { useState } from 'react';
import { LoginCredentials } from "../../api/authApi"
import CloseFormButton from '../CloseFormButton';
import Error from '../profile/Error';
import LoadButton from '../LoadButton';
import { GoogleLogin } from '@react-oauth/google';
interface LoginProps {
    handleModalClose: (event: React.MouseEvent<HTMLButtonElement>) => void;
    handleLogin: (loginCredentials: LoginCredentials) => void;
    handleGoogleLogin: (googleToken: string) => void;
    isOpen: boolean;
    error?: Error | null;
    loading: boolean;
};

export default function Login(props: LoginProps) {
    
    const [loginCredentials, setLoginCredentials] = useState<LoginCredentials>({
        email: "",
        password: ""
    });

    function login(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        props.handleLogin(loginCredentials);
    }

    function handleInput(event: React.ChangeEvent<HTMLInputElement>) {
        const {name, value} = event.target;
        setLoginCredentials((prevState) => {
            return {...prevState, [name]: value};
        })
    }

    function handleGoogleLogin(response: any) {
        console.log("handleGoogleLogin: " + response)
        try{
            const googleToken = response.credential;
            props.handleGoogleLogin(googleToken);
        } catch(e) {
            console.error(e);
        }
    }

    return(
        <form className={`login ${props.isOpen ? 'form-appear' : 'form-disappear closing'}`} onSubmit={(e) => login(e)}>
            <CloseFormButton handleClose={(e) => props.handleModalClose(e)}/>
            <div className='login-container'>
                <h1 className="login__header">Log In</h1>
                {props.error && <Error error={props.error}/>}
                <input className="login__input" placeholder="Email" value={loginCredentials.email}  name="email" onChange={handleInput} required></input>
                <input className="login__input" type="password" value={loginCredentials.password}  name="password" onChange={handleInput} placeholder="Password" required></input>
                <a className="login__forgot_password" href="/accounts/password/forgot">Forgot your password?</a>

                <div className="google-login-container">
                    <GoogleLogin onSuccess={handleGoogleLogin} onError={() => console.log("Google Login Failed")}></GoogleLogin>
                </div>

                <LoadButton innertText='Sign in' loading={props.loading}/>
            </div>
        </form>
    );
}