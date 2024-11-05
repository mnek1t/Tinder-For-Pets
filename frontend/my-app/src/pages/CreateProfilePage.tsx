import CreateProfileForm from "../components/profile/CreateProfileForm"
import EmptyHeader from "../components/EmptyHeader";
import {createPetProfile, ProfileData} from "../api/profileApi";

export default function CreateProfilePage() {
    document.body.style.background = '#FEFAF6';
    function createProfile(event : React.FormEvent<HTMLFormElement>, data: ProfileData) {
        event.preventDefault();
        console.log(event)
        createPetProfile(data);
    }

    return(
        <>
            <EmptyHeader/>
            <CreateProfileForm createProfile={createProfile}/>
        </>
    );
}