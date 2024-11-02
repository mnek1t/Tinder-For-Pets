import React, {useState, useEffect} from 'react'
import {ResetPasswordCredentials} from '../api/authApi';
import "../styles/form.css"
import { resetPassword } from '../api/authApi';
import { useLocation } from 'react-router-dom';
import ConfirmedWindow from './ConfirmedWindow';
export default function ResetPasswordForm() {
    const location = useLocation();
    const queryParams = new URLSearchParams(location.search);
    useEffect(() => {
        const tokenFromQueryParam = queryParams.get('token') || "token"
        setResetPasswordCreds(prevState => {
            return {
                ...prevState,
                token: tokenFromQueryParam
            };
        });
    }, [])

    const [resetPasswordCreds, setResetPasswordCreds] = useState<ResetPasswordCredentials>({
        newPassword: "",
        confirmPassword: "",
        token: ""
    });    
    
    const [resetResult, setResetResult] = useState({
        isSuccess: false,
        message: ""
    });

    function handleResetPassword(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();
        console.log(resetPasswordCreds)
        resetPassword(resetPasswordCreds)
        .then((data) => {
            setResetResult(prevState => {
                return({...prevState, message:data, isSuccess : true});
            })
        })
        .catch((error) => {
            setResetResult(prevState => {
                return({...prevState, message:error, isSuccess : false});
            })
        })
    }
    
    function handleInput(e: React.ChangeEvent<HTMLInputElement>) {
        const {name, value} = e.target;
        setResetPasswordCreds(prevState => {
            return {...prevState, [name]: value}
        })
    }

    return(
        <form className='reset-password' onSubmit={(e) => handleResetPassword(e)}>
            <div className='reset-password-container'>
                {resetResult.isSuccess ? (<ConfirmedWindow title={resetResult.message} message="Click <Back to login> to log in with new password" />) : 
                (<>
                    <h1 className="reset-password__header">Reset your password</h1>
                    <p className="forgot-password-hint">Enter new password and confirm it</p>
                    <input type="password" className="reset-password__input" placeholder="Password" value={resetPasswordCreds.newPassword}  name="newPassword" onChange={handleInput} required></input>
                    <input type="password" className="reset-password__input" placeholder="Confirm password" value={resetPasswordCreds.confirmPassword}  name="confirmPassword" onChange={handleInput} required></input>
                    <button className="reset-password__button" type='submit'>Reset Password</button>
                    <br/>
                    <div className="or-alt">
                        <div className="line"></div>
                        <div className="text">OR</div>
                        <div className="line"></div>
                    </div>
                </>)}
            </div>
            <div className="back-2-login-container">
                <a className="back-2-login-link" href="/login">Back to login</a>
            </div>
        </form>
    );
}