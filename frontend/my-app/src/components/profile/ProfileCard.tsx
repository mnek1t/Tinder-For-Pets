import { getProfileImage } from "../../api/profileApi";
import { useState, useEffect } from "react";
export default function ProfileCard()
{
    const [image, setImage] = useState<JSX.Element>();
    useEffect(() => {
        getProfileImage()
        .then(data => {
            setImage(<img className="animal-image" src={`data:${data.contentType};base64,${data.fileContents}`} alt="Animal"/>)
        })
    }, [])
    return(
        <div className="profile-card">
            {image}
            <h1>Name, age</h1>
            <p className="title">Type, Breed</p>
            <p>Gender</p>
            <p>Is Vacinated</p>
            <p>Is Sterilized</p>
        </div>
    );
}