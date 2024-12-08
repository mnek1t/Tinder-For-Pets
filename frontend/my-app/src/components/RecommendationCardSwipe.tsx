import { motion, useMotionValue, useTransform} from 'framer-motion'
import { RecomendationCard } from '../pages/RecommendationPage'
import { Dispatch, SetStateAction } from 'react';
import infoIcon  from "../assets/info-circle-svgrepo-com.svg"
interface CardSwipeProps {
    id: string
    name: string,
    age: number,
    imageData: string, 
    imageFormat: string,
    cards: RecomendationCard[],
    setCards: Dispatch<SetStateAction<RecomendationCard[]>>
    handleSwipe: (profileId: string, isLike: boolean) => void
}

const RecommendationCardSwipe = (props : CardSwipeProps) => {
    const motionValue = useMotionValue(0);
    const rotateValue = useTransform(motionValue, [-200, 200], [-18, 18]);
    const opacityValue = useTransform(
        motionValue,
        [-200, -150, 0, 150, 200],
        [0, 1, 1, 1, 0]
    );

    function handleOnDragEnd() {
        const swipeDistance = motionValue.get()
        if(Math.abs(swipeDistance) > 50) {
            const isRightSwipe = swipeDistance > 0;
            props.handleSwipe(props.id, isRightSwipe);
            props.setCards(prevState => prevState.filter((card) => card.id !== props.id))
        } 
    }
    return (
        <>
        <motion.div  
            drag="x"
            dragConstraints={{
                left:0,
                right:0
            }}
            onDragEnd={handleOnDragEnd}
            whileTap={{ scale: 0.95 }}
            style={{
                x: motionValue,
                opacity: opacityValue,
                rotate: rotateValue,
                boxShadow: '0 4px 8px rgba(0,0,0,0.2)',
                position:"relative",
                borderRadius: "8px"
            }}>
                <div className="recs-img-wrapper">
                    <img className='recommended-profile' src={`data:${props.imageFormat};base64,${props.imageData}`} alt="Recommended profile"/>
                    <div className='profile-info'>
                        <div className='profile-info__general'>
                            <h1 className="profile-name">{props.name}</h1>
                            <h1 className="profile-age">{props.age}</h1>
                        </div>
                        <div className="hint">
                            <img className='info-button' src={infoIcon} alt="info-icon"/>
                        </div>
                    </div>
                </div>
        </motion.div>    
        </>
    );
};

export default RecommendationCardSwipe;