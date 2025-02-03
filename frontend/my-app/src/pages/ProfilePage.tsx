
// TODO: IMPLEMENT GET DATA IN PAGE AND IF ERROR DO NOT RENDER ANYTHING
import React, { useState, useEffect } from "react";
import Sidebar from "../components/Sidebar";
import ProfileCard from "../components/profile/ProfileCard";
import FormPageWrapper from '../components/FormPageWrapper';
import ConfirmationPopup from "../components/ConfirmationPopup";
import ProfileEditForm from "../components/ProfileEditForm";
import { logout, deleteAccount } from "../api/authApi";
import { getProfileDetails, uploadProfileImage, updatePetProfile, ProfileDetailsData, getAnimalTypes, getBreeds, UpdateAnimalProfileRequest } from "../api/profileApi";
import { useNavigate } from "react-router-dom";
import { CircularProgress } from "@mui/material";
import { DEFAULT_ANIMAL_TYPE_ID } from "../utils/TinderConstants";

const deleteAccountDescription = "If you delete your account, you will permanently lose your profile, messages, photos and matches. If you delete your account, this action cannot be undone."

export default function ProfilePage() {
    const [animalTypes, setAnimalTypes] = useState([]);
    const [breeds, setBreeds] = useState([]);
    const [distance, setDistance] = useState(50);
    const [showConfirmationWindow, setShowConfirmationWindow ] = useState<boolean>(false);
    const [profileDetails, setProfileDetails] = useState<ProfileDetailsData | null>(null);
    const [loading, setLoading] = useState(false);
    const [isEditing, setIsEditing] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
            getAnimalTypes()
              .then((animalTypes) => {
                setAnimalTypes(animalTypes);
                handleFetchBreeds(animalTypes[0]?.id || profileDetails?.animal.type);
            })
            .catch((err) => console.error(err));
        }, []);
    const handleFetchBreeds = (typeId: number) => {
        getBreeds(typeId)
            .then(setBreeds)
            .catch((err) => console.error(err));
    };
    useEffect(() => {
        document.body.style.background = '#FEFAF6';
        setLoading(true);
            getProfileDetails()
            .then(({ profile, animal, images }) => {
            setProfileDetails({
                    animal: {
                        id: animal?.id,
                        type: animal?.animalType || "", 
                        breed: animal?.breed || "", 
                    },
                    profile: {
                        id: profile?.id || "", 
                        name: profile?.name || "",
                        description: profile?.description || "",
                        age: profile?.age || 0,
                        sex: profile?.sex || "",
                        dateOfBirth: profile?.dateOfBirth || "",
                        isVaccinated: profile?.isVaccinated || false,
                        isSterilized: profile?.isSterilized || false,
                        city: profile?.city,
                        country: profile?.country
                    },
                    images: [
                    {
                        imageData: images[0]?.imageData || "", 
                        imageFormat: images[0]?.imageFormat || "", 
                    },
                    ],
                });
            })
            .catch(error => {
                console.error('Error fetching profile details:', error);
            })
            .finally(() => {
                setLoading(false);
            })
        return () => {
            document.body.style.backgroundImage = "";
            document.body.style.backgroundSize = '';
        };
    }, [isEditing]);
    
    function handleLogout(event : React.MouseEvent<HTMLButtonElement>) {
        setLoading(true);
        logout()
        .then(() => {
            navigate("/about");
        })
        .catch((error) => {
            console.error(error);
        })
        .finally(() => setLoading(false));
        
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
            setLoading(true);
            deleteAccount()
            .then(()=> {
                navigate("/about");
                setShowConfirmationWindow(false);
            })
            .catch((error) => console.error(error))
            .finally(()=> setLoading(false));
        }
    }

    function handleClickEditProfileCard() {
        setIsEditing((prev) => !prev);
    }

    function handleToggleSwitch(event : React.ChangeEvent<HTMLInputElement>) {
        const {name, checked} = event.target;
        setProfileDetails((prev) => {
            if (!prev) return null; 
        
            return {
              ...prev,
              profile: {
                ...prev.profile,
                [name]: checked, 
              },
            };
          });
    }

    function handleImageUpdate(file: File): Promise<any> {
        const request = {
            file: file,
            description: "test",
        };
        return uploadProfileImage(request) // Return the promise here
            .then((response) => {
                console.log(response);
                return response;
            })
            .catch(error => {
                console.error(error);
                throw error; // Re-throw the error to handle it in the calling function
            });
    }

    function handleProfileUpdate(request: UpdateAnimalProfileRequest): Promise<any> {
    if (!profileDetails?.profile.id) {
        return Promise.reject(new Error("Profile ID is missing")); // Return a rejected promise if the ID is missing
    }

    return updatePetProfile(profileDetails.animal.id, request) // Return the promise here
        .then((response) => {
            console.log(response);
            return response;
        })
        .catch(error => {
            console.error(error);
            throw error; // Re-throw the error to handle it in the calling function
        });
}


    return(
        <>
            <div className="container">
                {loading ? (
                    <FormPageWrapper title="delete-account" showHeader={false}>
                        <CircularProgress/> 
                    </FormPageWrapper>) :
                    (
                        <>
                            <div className="sidebar-container">
                                <Sidebar 
                                    isUserPreferences = {true}
                                    handleLogout={handleLogout} 
                                    handleDeleteAccount={handleDeleteAccountButtonClick} 
                                    distance={distance}/>
                            </div>
                            
                            <div className="profile">
                                {isEditing ? <ProfileEditForm 
                                                animalTypes={animalTypes} 
                                                breeds={breeds} 
                                                profileDetails={profileDetails}
                                                handleClickEditProfileCard={handleClickEditProfileCard}
                                                handleToggleSwitch={handleToggleSwitch} 
                                                handleImageUpdate={handleImageUpdate} 
                                                handleProfileUpdate={handleProfileUpdate}
                                            /> : 
                                            <ProfileCard 
                                                profileDetails={profileDetails} 
                                                loading={loading} 
                                                handleClickEditProfileCard={handleClickEditProfileCard}
                                            />}
                            </div>
                            {
                                showConfirmationWindow && 
                                <FormPageWrapper title="delete-account" showHeader={false}>
                                    <ConfirmationPopup question="You sure you want to delete an account?" description={deleteAccountDescription} handleAction={handleDeleteAccount} loading={loading}/> 
                                </FormPageWrapper>
                            }
                        </>
                    )
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