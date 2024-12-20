import '../../styles/form.css';
import { useState } from 'react';
import {RegisterCredentials} from "../../api/authApi"
import CloseFormButton from '../CloseFormButton';
import Error from '../profile/Error';
import LoadButton from '../LoadButton';
interface RegisterProps {
    handleRegister: (loginCredentials: RegisterCredentials) => void;
    handleModalClose: (event: React.MouseEvent<HTMLButtonElement>) => void;
    isOpen: boolean;
    error?: Error | null;
    loading: boolean
};

export default function RegisterForm(props:  RegisterProps) {
    const [registerCredentials, setRegisterCredentials] = useState<RegisterCredentials>({
        username: "",
        email: "",
        password: ""
    });

    function register(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        props.handleRegister(registerCredentials)
    }

    function handleInput(event: React.ChangeEvent<HTMLInputElement>) {
        const {name, value} = event.target;
        setRegisterCredentials((prevState) => {
            return {...prevState, [name]: value};
        })
    }

    return(
        <form className={`register ${props.isOpen ? 'form-appear' : 'form-disappear closing'}`} onSubmit={(e) => register(e)}>
            <CloseFormButton handleClose={props.handleModalClose}/>
            <div className='register-container'>
                <h1 className="register__header">Create Account</h1>
                {props.error && <Error error={props.error}/>}
                <input className="register__input" placeholder="Username" value={registerCredentials.username} name="username" onChange={(e) => handleInput(e)} required></input>
                <input className="register__input" placeholder="Email" value={registerCredentials.email}  name="email" onChange={(e) => handleInput(e)} required></input>
                <input className="register__input" type="password" value={registerCredentials.password}  name="password" onChange={(e) => handleInput(e)} placeholder="Password" required></input>
                <LoadButton innertText='Register' loading={props.loading}/>
            </div>
        </form>
    );
}