import '../styles/form.css';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {register, RegisterCredentials} from "../api/authApi"
import CloseFormButton from './CloseFormButton';

interface RegisterProps {
    handleModalClose: (event: React.MouseEvent<HTMLButtonElement>) => void;
    isOpen: boolean;
};

export default function RegisterForm(props:  RegisterProps) {
    const [error, setError] = useState<string | null>(null);

    const [registerCredentials, setRegisterCredentials] = useState<RegisterCredentials>({
        username: "",
        email: "",
        password: ""
    });

    const navigate = useNavigate();

    function makeRegister(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        console.log("Registering");
        register(registerCredentials)
        .then(() => {
            navigate("/about");
            setError(null);
        })
        .catch(error => {
            console.error('Register failed:', error);
            setError(error.message);
        });
    }

    function handleInput(event: React.ChangeEvent<HTMLInputElement>) {
        const {name, value} = event.target;
        setRegisterCredentials((prevState) => {
            return {...prevState, [name]: value};
        })
    }

    return(
        <form className={`register ${props.isOpen ? 'form-appear' : 'form-disappear closing'}`} onSubmit={(e) => makeRegister(e)}>
            <CloseFormButton handleClose={(e) => props.handleModalClose(e)}/>
            <div className='register-container'>
                <h1 className="register__header">Create Account</h1>
                <input className="register__input" placeholder="Username" value={registerCredentials.username} name="username" onChange={(e) => handleInput(e)} required></input>
                <input className="register__input" placeholder="Email" value={registerCredentials.email}  name="email" onChange={(e) => handleInput(e)} required></input>
                <input className="register__input" type="password" value={registerCredentials.password}  name="password" onChange={(e) => handleInput(e)} placeholder="Password" required></input>
                <button className="register__button" type='submit'>Register</button>
            </div>
        </form>
    );
}