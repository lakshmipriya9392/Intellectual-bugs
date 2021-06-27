import React, { useState } from 'react'
import { Link } from 'react-router-dom'
import './../../App.css'
import BackPage from './../../assets/4380.jpg'
import { motion } from 'framer-motion'

function frontPage(props) {

    // const [signup, setSignUp] = useState(false)

    return (
        <div className='w-screen h-screen overflow-hidden'>
            <motion.div
                initial={{ marginTop: "-100vh" }}
                animate={{ marginTop: 0 }}
                transition={{ duration: 1, type: 'spring', stiffness: 150 }}
            >
                <div className='relative top-0 left-0 right-0 bottom-0'>

                    <img src={BackPage} alt="" className="absolute

top-0 bottom-0 left-0 right-0 w-screen h-screen z-0 " />

                    {/* <img src={BackPage} alt="" className="w-screen h-1/2 sm:h-screen absolute top-0 left-0 right-0 bottom-0 overflow-hidden" /> */}

                    <div className="absolute top-2 right-2 flex mt-2 mr-4">
                        <Link to='/signIn'>
                            <div className="mx-2 border-2 rounded-xl 
                px-4 py-2 border-blue-500 text-blue-500 cursor-pointer
                 duration-200 hover:bg-blue-500 hover:text-white text-2xl bg-white z-50" >Sign in</div></Link>

                        <Link to='/signUp'>
                            <motion.div
                                initial={{ scale: 1 }}
                                animate={{ scale: 1 }}

                                className="mx-2 border-2 rounded-xl px-4 py-2
                 border-blue-500 text-blue-500 cursor-pointer duration-200
                  hover:bg-blue-500 hover:text-white text-2xl bg-white z-50"

                            // onClick={ }

                            >Sign up</motion.div>
                        </Link>
                    </div>

                    <motion.div

                        animate={{ marginLeft: [0, 30, -30, 30, -30, 30, -30, 30, -30, 0] }}
                        transition={{ duration: 1, type: 'spring', stiffness: 120, delay: 1.8 }}


                        className="absolute left-0 right-0 sm:top-72 top-48 flex justify-center ">
                        <Link to="/selection">
                            <div className="border-2 rounded-xl px-4 py-2 border-blue-500
                 text-blue-500 cursor-pointer duration-200 hover:bg-blue-500
                  hover:text-white text-2xl bg-white z-50" onClick={props.next}> Get Started </div>
                        </Link>
                    </motion.div>

                </div>

            </motion.div></div>
    )
}
export default frontPage
