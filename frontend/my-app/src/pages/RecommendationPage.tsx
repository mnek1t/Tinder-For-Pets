import "../styles/swipe.css"
import {useEffect, useState} from "react"
import Sidebar from "../components/Sidebar";
import RecommendationCardSwipe from "../components/RecommendationCardSwipe";
import { getProfileRecommendations, createSwipe } from "../api/profileApi"; 
import { CircularProgress } from "@mui/material";
export type RecomendationCard =  {
    id: string
    name: string,
    age: number,
    imageData: string,
    imageFormat: string
}
export default function RecommendationPage() {
    const [loading, setLoading] = useState(false);
    const [recommendationCards, setRecommendationCards] = useState<RecomendationCard[]>([])
    useEffect(() => {
        document.body.style.background = '#FEFAF6';
        setLoading(true);
        getProfileRecommendations()
        .then(data => {
            const cards : RecomendationCard[] = data.map(card => ({
                id: card.profile.id,
                name : card.profile.name,
                age : card.profile.age,
                imageData: card.images[0].imageData,
                imageFormat: card.images[0].imageFormat,
            })) 
            setRecommendationCards(cards);
        })
        .catch((error) => console.error(error))
        .finally(() => setLoading(false));
        
        return () => {
            document.body.style.backgroundImage = "";
            document.body.style.backgroundSize = '';
        };
    }, []);

    function handleSwipe(profileId: string, isLike: boolean) {
        createSwipe(profileId, isLike)
    }

    return(
        <>
        <div className="container">
            <div className="sidebar-container">
                <Sidebar isUserPreferences={false}/>
            </div>
            { loading ? (<CircularProgress sx={{ color: "#864c4c", margin: 'auto auto' }} size={48} />) : 
                (recommendationCards && recommendationCards.length !== 0 ? 
                (<div className="swiping-profiles">
                    {recommendationCards.map((card) => {
                        return <RecommendationCardSwipe key={card.id} {...card} cards={recommendationCards} setCards={setRecommendationCards} handleSwipe={handleSwipe}/>
                    })}
                </div>) : (<div>NOTHING</div>)
            )}
            
        </div>
            
        </>
    );
}