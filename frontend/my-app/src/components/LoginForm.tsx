import '../styles/form.css';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {login, LoginCredentials} from "../api/authApi"
import CloseFormButton from './CloseFormButton';

interface LoginProps {
    handleModalClose: (event: React.MouseEvent<HTMLButtonElement>) => void;
    isOpen: boolean;
};

export default function Login(props: LoginProps) {
    const [error, setError] = useState<string | null>(null);
    const [loginCredentials, setLoginCredentials] = useState<LoginCredentials>({
        email: "",
        password: ""
    });

    const navigate = useNavigate();

    function makeLogin(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        console.log("Loggining");
        login(loginCredentials)
        .then(() => {
            navigate("/about");
            setError(null);
        })
        .catch(error => {
            console.error('Login failed:', error);
            setError(error.message);
        });
    }

    function handleInput(event: React.ChangeEvent<HTMLInputElement>) {
        const {name, value} = event.target;
        setLoginCredentials((prevState) => {
            return {...prevState, [name]: value};
        })
    }

    return(
        <form className={`login ${props.isOpen ? 'form-appear' : 'form-disappear closing'}`} onSubmit={(e) => makeLogin(e)}>
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