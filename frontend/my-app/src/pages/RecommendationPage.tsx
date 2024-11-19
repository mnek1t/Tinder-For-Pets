import "../styles/swipe.css"
import {useEffect, useState} from "react"
import Sidebar from "../components/Sidebar";
import RecommendationCardSwipe from "../components/RecommendationCardSwipe";
import { getProfileRecommendations, createSwipe } from "../api/profileApi"; 
export type RecomendationCard =  {
    id: string
    name: string,
    age: number,
    imageData: string,
    imageFormat: string
}
export default function RecommendationPage() {
    const [recommendationCards, setRecommendationCards] = useState<RecomendationCard[]>([])
    useEffect(() => {
        document.body.style.background = '#FEFAF6';
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
            { recommendationCards && <div className="swiping-profiles">
                {recommendationCards.map((card) => {
                    return <RecommendationCardSwipe key={card.id} {...card} cards={recommendationCards} setCards={setRecommendationCards} handleSwipe={handleSwipe}/>
                })}
            </div>}
            
        </div>
            
        </>
    );
}