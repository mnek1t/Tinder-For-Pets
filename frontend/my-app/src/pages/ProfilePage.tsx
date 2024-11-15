
// TODO: IMPLEMENT GET DATA IN PAGE AND IF ERROR DO NOT RENDER ANYTHING
import Sidebar from "../components/Sidebar";
import ProfileCard from "../components/profile/ProfileCard";
import FormPageWrapper from '../components/FormPageWrapper';
import { deleteAccount } from "../api/authApi";
// import { useState, useEffect } from "react";
import { logout } from "../api/authApi"
import { useNavigate } from "react-router-dom";
import React, { useState, useEffect } from "react";
import ConfirmationPopup from "../components/ConfirmationPopup";

const deleteAccountDescription = "If you delete your account, you will permanently lose your profile, messages, photos and matches. If you delete your account, this action cannot be undone."

export default function ProfilePage() {
    useEffect(() => {
        document.body.style.background = '#FEFAF6';

        return () => {
            document.body.style.backgroundImage = "";
            document.body.style.backgroundSize = '';
        };
    }, []);
    const navigate = useNavigate();
    const [showConfirmationWindow, setShowConfirmationWindow ] = useState<boolean>(false);

    function handleLogout(event : React.MouseEvent<HTMLButtonElement>) {
        logout();
        navigate("/about");
    }

    function handleDeleteAccountButtonClick(event : React.MouseEvent<HTMLButtonElement>) {
        setShowConfirmationWindow(true);
    }

    function handleDeleteAccount(event : React.MouseEvent<HTMLButtonElement>) {
        const {name} = event.target as HTMLButtonElement;
        if(name === 'cancel') 
        {
            setShowConfirmationWindow(false);
        } else if(name === 'confirm'){
            deleteAccount();
            navigate("/about");
            setShowConfirmationWindow(false);
        }
    }

    return(
        <>
            <div className="container">
                <div className="sidebar-container">
                    <Sidebar handleLogout={handleLogout} handleDeleteAccount={handleDeleteAccountButtonClick}/>
                </div>
                
                <div className="profile">
                    <ProfileCard />
                </div>
                {
                    showConfirmationWindow && 
                    <FormPageWrapper title="delete-account" showHeader={false}>
                        <ConfirmationPopup question="Hello Ruslan" description={deleteAccountDescription} handleAction={handleDeleteAccount}/> 
                    </FormPageWrapper>
                }
            </div>       
            {/* {isDataReceived && <div className="container">
                <div className="sidebar-container">
                    <Sidebar handleLogout={handleLogout} handleDeleteAccount={handleDeleteAccount}/>
                </div>
                
                <div className="profile">
                    <ProfileCard />
                </div>
            </div>} */}
        </>
        );
}