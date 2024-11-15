import '../../styles/form.css';
import { useState } from 'react';
import {LoginCredentials} from "../../api/authApi"
import CloseFormButton from '../CloseFormButton';

interface LoginProps {
    handleModalClose: (event: React.MouseEvent<HTMLButtonElement>) => void;
    handleLogin: (loginCredentials: LoginCredentials) => void;
    isOpen: boolean;
};

export default function Login(props: LoginProps) {
    
    const [loginCredentials, setLoginCredentials] = useState<LoginCredentials>({
        email: "",
        password: ""
    });

    function login(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        console.log("Loggining");
        props.handleLogin(loginCredentials);
    }

    function handleInput(event: React.ChangeEvent<HTMLInputElement>) {
        const {name, value} = event.target;
        setLoginCredentials((prevState) => {
            return {...prevState, [name]: value};
        })
    }

    return(
        <form className={`login ${props.isOpen ? 'form-appear' : 'form-disappear closing'}`} onSubmit={(e) => login(e)}>
            <CloseFormButton handleClose={props.handleModalClose}/>
            <div className='login-container'>
                <h1 className="login__header">Log In</h1>
                <input className="login__input" placeholder="Email" value={loginCredentials.email}  name="email" onChange={handleInput} required></input>
                <input className="login__input" type="password" value={loginCredentials.password}  name="password" onChange={handleInput} placeholder="Password" required></input>
                <a className="login__forgot_password" href="/accounts/password/forgot">Forgot your password?</a>
                <button className="login__button" type='submit'>Sign In</button>
            </div>
        </form>
    );
}