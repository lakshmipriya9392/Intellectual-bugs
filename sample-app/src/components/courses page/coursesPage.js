import React, { useCallback } from 'react'
import './../../App.css'
// import { Redirect, Route } from 'react-router-dom'
import Navbar from './../navbar/navbar'
import CoursesBackPage from './../../assets/19362653.jpg'
import { motion } from 'framer-motion'
// import ReactPage from './react'
import { useHistory } from "react-router-dom"
// import Angular from './Angular'
import ReactImage from './../../assets/ReactImage.png'
import CSharpImage from './../../assets/CSharp.jpg'
import BlenderImage from './../../assets/Blender.jpg'
import MongoDBImage from './../../assets/MongoDB.jpg'
import NodejsImage from './../../assets/Nodejs.png'
import AngularImage from './../../assets/Angular.png'

function CoursesPage() {

    const courses = [
        {
            id: 1,
            name: "React",
            location: '/courses/react',
            image: ReactImage
        },
        {
            id: 2,
            name: "c#",
            location: '/courses/csharp',
            image: CSharpImage
        },
        {
            id: 3,
            name: "Blender",
            location: '/courses/blender',
            image: BlenderImage
        },
        {
            id: 4,
            name: "MongoDB",
            location: '/courses/mongodb',
            image: MongoDBImage
        },
        {
            id: 5,
            name: "Node js",
            location: '/courses/node',
            image: NodejsImage
        }, ,
        {
            id: 6,
            name: "Angular",
            location: '/courses/angular',
            image: AngularImage
        }
    ]

    const history = useHistory();


    return (

        <div className='w-screen overflow-x-hidden'>


            <motion.div
                initial={{ marginLeft: '100vw' }}
                animate={{ marginLeft: 0 }}
                transition={{ duration: 0.5, type: 'spring', stiffness: 200, delay: 0.2 }}
            >
                <div className="relative top-0 left-0 right-0 bottom-0 h-screen w-screen">
                    <Navbar />
                    <img src={CoursesBackPage} alt="" className="absolute

             top-0 bottom-0 left-0 right-0 w-screen h-screen z-0 " />

                    <div className="flex flex-wrap justify-center items-center m-2 bg-transparent absolute z-10">
                        {courses.map(course => {
                            return (
                                <motion.div
                                    whileHover={{ scale: 1.05 }}
                                    whileTap={{ scale: 0.95 }}
                                    key={course.id}
                                    className="m-3 bg-white min-w-min w-80 h-60 rounded-3xl cursor-pointer flex flex-col shadow-2xl"
                                    onClick={() => history.push(course.location)}
                                >
                                    <img src={course.image} alt="" className='rounded-t-3xl h-3/5 border-b-2' />
                                    <div className=" text-center text-2xl my-5">{course.name}</div>
                                </motion.div>

                            )
                        })}
                    </div>
                </div>
            </motion.div></div >


    )
}

export default CoursesPage
